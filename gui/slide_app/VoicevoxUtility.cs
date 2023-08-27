using System.Collections.Generic;
using System.Media;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace slide_app
{
    class VoicevoxUtility
    {
        const string baseUrl = "http://127.0.0.1:50021/"; // localhostだとレスポンスが遅いのアドレス指定
        private static readonly HttpClient httpClient = new HttpClient();

        private static Dictionary<string, int> VoiceName_dic = new Dictionary<string, int>(){
　　　　　　{"四国めたん",2},
　　　　　　{"ずんだもん",3},
　　　　　　{"春日部つむぎ",8},
            {"波音リツ",9},
            {"雨晴はう",10},
            {"玄野武宏",11},
            {"白上虎太郎",12},
            {"青山龍星",13},
            {"冥鳴ひまり",14},
            {"九州そら",16},
            {"もち子さん",20},
            {"剣崎雌雄",21},
            {"WhiteCUL",23},
            {"後鬼",27},
            {"No.7",29}
        };

    public class VOICEVOX_query
        {
            public Accent_phrases[] accent_phrases { get; set; }
            public double speedScale { get; set; }
            public double pitchScale { get; set; }
            public double intonationScale { get; set; }
            public double volumeScale { get; set; }
            public double prePhonemeLength { get; set; } //音声前の無音時間
            public double postPhonemeLength { get; set; } //音声後の無音時間
            public int outputSamplingRate { get; set; }
            public bool outputStereo { get; set; }
            public string kana { get; set; }
        }

        public class Accent_phrases
        {
            public Moras[] moras { get; set; }
            public int accent { get; set; }
            public Moras? pause_mora { get; set; }
            public bool is_interrogative { get; set; }
        }

        public class Moras
        {
            public string text { get; set; }
            public string? consonant { get; set; }
            public double? consonant_length { get; set; }
            public string? vowel { get; set; }
            public double vowel_length { get; set; }
            public double pitch { get; set; }
        }


        public static async Task Speek(string text, string speaker)
        {
            string query = await CreateAudioQuery(text, VoiceName_dic[speaker]);

            // 音声合成
            using var request = new HttpRequestMessage(new HttpMethod("POST"), $"{baseUrl}synthesis?speaker={VoiceName_dic[speaker]}&enable_interrogative_upspeak=true");
            request.Headers.TryAddWithoutValidation("accept", "audio/wav");

            request.Content = new StringContent(query);
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            var response = await httpClient.SendAsync(request);

            // 音声再生
            using var httpStream = await response.Content.ReadAsStreamAsync();
            var player = new SoundPlayer(httpStream);
            player.PlaySync();
        }

        public static async Task RecordSpeech(string outputWaveFilePath, string text, string speaker)
        {
            string query = await CreateAudioQuery(text, VoiceName_dic[speaker]);
            VOICEVOX_query query2 = JsonSerializer.Deserialize<VOICEVOX_query>(query);
            query2.speedScale = (double)Form1.Speed;
            query2.intonationScale = (double)Form1.Intonation;
            query2.prePhonemeLength = (double)Form1.prePhonemeLength;
            query2.postPhonemeLength = (double)Form1.postPhonemeLength;
            query = JsonSerializer.Serialize<VOICEVOX_query>(query2);

            // 音声合成
            using var request = new HttpRequestMessage(new HttpMethod("POST"), $"{baseUrl}synthesis?speaker={VoiceName_dic[speaker]}&enable_interrogative_upspeak=true");
            request.Headers.TryAddWithoutValidation("accept", "audio/wav");

            request.Content = new StringContent(query);
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            var response = await httpClient.SendAsync(request);

            // 書き出し
            using var fs = System.IO.File.Create(outputWaveFilePath);
            using var stream = await response.Content.ReadAsStreamAsync();
            stream.CopyTo(fs);
            fs.Flush();
        }

        private static async Task<string> CreateAudioQuery(string text, int speakerId)
        {
            using var requestMessage = new HttpRequestMessage(new HttpMethod("POST"), $"{baseUrl}audio_query?text={text}&speaker={speakerId}");
            requestMessage.Headers.TryAddWithoutValidation("accept", "application/json");

            requestMessage.Content = new StringContent("");
            requestMessage.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");
            var response = await httpClient.SendAsync(requestMessage);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
