from ytmusicapi import YTMusic
import re

class YTMusicClient:
    """
    Handles:
    - Authentication
    - Searching (multi‑stage fallback)
    - Link parsing
    - Metadata extraction
    - Safe genre detection
    """

    def __init__(self, auth_path="headers_auth.json"):
        try:
            self.yt = YTMusic(auth_path)
        except Exception as e:
            raise RuntimeError(f"Failed to load YTMusic authentication: {e}")

    # ---------------------------------------------------------
    # Public API
    # ---------------------------------------------------------

    def get_song_data(self, user_input):
        """
        Accepts either:
        - A song name
        - A YouTube Music link
        Returns a clean metadata dictionary.
        """

        # If user gave a link, extract videoId
        video_id = self._extract_video_id(user_input)

        # Otherwise search for it
        if not video_id:
            video_id = self._search_for_valid_video_id(user_input)

        if not video_id:
            raise ValueError("Could not find a playable song for that query.")

        return self._get_metadata(video_id)

    # ---------------------------------------------------------
    # Internal helpers
    # ---------------------------------------------------------

    def _extract_video_id(self, text):
        """
        Extracts videoId from a YouTube Music link.
        Returns None if no link is found.
        """
        match = re.search(r"v=([A-Za-z0-9_-]{11})", text)
        return match.group(1) if match else None

    # ---------------------------------------------------------
    # Multi‑stage search
    # ---------------------------------------------------------

    def _search_for_valid_video_id(self, query):
        """
        Searches YouTube Music and returns the first videoId
        that corresponds to a playable song.
        Includes multiple fallback strategies.
        """

        # Pass 1: song search
        video_id = self._search_and_validate(query, filter_type="songs")
        if video_id:
            return video_id

        # Pass 2: video search
        video_id = self._search_and_validate(query, filter_type="videos")
        if video_id:
            return video_id

        # Pass 3: raw search
        video_id = self._search_and_validate(query, filter_type=None)
        if video_id:
            return video_id

        # Pass 4: brute-force fallback
        video_id = self._bruteforce_web_search(query)
        if video_id:
            return video_id

        return None

    def _search_and_validate(self, query, filter_type):
        """
        Helper that searches with a given filter and validates results.
        """
        try:
            if filter_type:
                results = self.yt.search(query, filter=filter_type)
            else:
                results = self.yt.search(query)

            if not results:
                return None

            for item in results:
                video_id = item.get("videoId")
                if not video_id:
                    continue

                # Validate metadata
                data = self.yt.get_song(video_id)
                if "videoDetails" in data:
                    return video_id

            return None

        except Exception:
            return None

    def _bruteforce_web_search(self, query):
        """
        Last-resort fallback using YouTube Music's raw search behavior.
        This often returns playable videos even when API filters fail.
        """
        try:
            results = self.yt.search(query)
            for item in results:
                video_id = item.get("videoId")
                if video_id:
                    return video_id
            return None
        except Exception:
            return None


    def _get_metadata(self, video_id):
        """
        Fetches metadata using watch playlist (more reliable than get_song).
        Attempts multiple fallbacks for duration.
        """
        try:
            data = self.yt.get_watch_playlist(video_id)
        except Exception as e:
            raise RuntimeError(f"Failed to fetch song metadata: {e}")

        if "tracks" not in data or not data["tracks"]:
            raise RuntimeError("This result is not a playable song.")

        track = data["tracks"][0]

        title = track.get("title")
        artist = track.get("artists", [{}])[0].get("name")

        # Duration fallbacks
        duration = (
            track.get("duration_seconds") or
            track.get("duration") or
            track.get("length") or
            None
        )

        # Default genre fallback
        DEFAULT_GENRE = "Unknown"
        genre = DEFAULT_GENRE  # watchPlaylist rarely includes genre

        return {
            "title": title,
            "artist": artist,
            "genre": genre,
            "video_id": video_id,
            "duration": duration
        }


    def _extract_genre(self, data):
        """
        Safely extracts genre from multiple possible locations.
        Returns None if no genre is found.
        """

        # Case 1: microformat
        if "microformat" in data:
            mf = data["microformat"].get("microformatDataRenderer", {})
            if "category" in mf:
                return mf["category"]

        # Case 2: musicGenres list
        if "musicGenres" in data:
            genres = data["musicGenres"]
            if isinstance(genres, list) and genres:
                return genres[0]

        return None