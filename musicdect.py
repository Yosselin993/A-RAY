from ytmusicapi import YTMusic

yt = YTMusic("headers_auth.json")

results = yt.search("Around the World", filter="songs")
video_id = results[0]["videoId"]

song_info = yt.get_song(video_id)

genre = None

if "microformat" in song_info:
    mf = song_info["microformat"].get("microformatDataRenderer", {})
    genre = mf.get("category")

if not genre:
    genre = song_info.get("musicGenres", [None])[0]

print("Genre:", genre)

