import json
import collections as cl
import re

INPUT_PATH = "Text/input_time.txt"

def get_input_array():
  """入力ファイルの文字列を行ごとに配列へ格納、返却

  Returns:
    list: 入力された文字列を行ごとに格納した配列
  """
  input_text = open(INPUT_PATH, "r", encoding="utf-8")
  lines = input_text.read().splitlines()
  input_text.close()
  return lines

def output_json (lyrics_char, spawn_time, action, color):
  ys = cl.OrderedDict()
  ys["lyrics"] = []

  for i in range(len(lyrics_char)):
    data = cl.OrderedDict()
    data["lyricsChar"] = lyrics_char[i]
    data["spawnTime"] = spawn_time[i]
    data["action"] = action[i]
    data["color"] = color[i]
    ys["lyrics"].append(data)

  fw = open("sugarsong.json", "w")
  json.dump(ys, fw, indent=2)

if __name__ == "__main__":
  input_array = get_input_array()

  lyrics_char = []
  spawn_time = []
  action = []
  color = []

  for i in range(len(input_array)):
    split_time = re.split(r'[\[\]]', input_array[i])
    del split_time[0]
    for j in range(len(split_time)):
      if (j % 2 == 0):
        spawn_time.append(split_time[j])
      else:
        lyrics_char.append(str(ord(split_time[j])))
        action.append("")
        color.append("")
    lyrics_char.append("")
    spawn_time.append("")
    action.append("")
    color.append("")
  
  output_json(lyrics_char, spawn_time, action, color)
