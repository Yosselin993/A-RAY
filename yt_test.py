from yt_client import YTMusicClient

yt = YTMusicClient()

user_input = input("Enter song name or YouTube Music link: ")

song = yt.get_song_data(user_input)

print("\nSong Data:")
for key, value in song.items():
    print(f"{key}: {value}")