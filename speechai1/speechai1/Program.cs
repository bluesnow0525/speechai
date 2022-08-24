using System;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using System.Windows;
using static System.Net.Mime.MediaTypeNames;

namespace MicrosoftSpeechSDKSamples
{
    class Program
    {
        private static string text;
        static async Task Main(string[] args)
        {
            String input;
            Console.WriteLine("Enter 1 to start, enter 0 to exit.\n");
            input = Console.ReadLine();
            while (input == "1")
            {
                await RecognitionWithMicrophoneAsync();
                await SynthesisToSpeakerAsync();
                Console.WriteLine("Enter 1 to start, enter 0 to exit.\n");
                input = Console.ReadLine();
            }
            Console.WriteLine("Bye.\n");
        }
        public static async Task SynthesisToSpeakerAsync()
        {
            String YourSubscriptionKey = "fed6cde6f90d4535b03975cb77d3d3fc";
            String YourServiceRegion = "eastasia";

            var config = SpeechConfig.FromSubscription(YourSubscriptionKey, YourServiceRegion);

            if (text == "哈囉。")
            {
                text = "吃大便";
            }
            else if (text == "你是誰？")
            {
                text = "我是蔡英文";
            }
            else if (text == "繞口令。")
            {
                text = "黑化黑灰化肥灰會揮發發灰黑諱為黑灰花會回飛，灰化灰黑化肥會揮發發黑灰為諱飛花回化為灰";
            }
            else
            {
                text = "三小";
            }
            var language = "zh-TW";
            config.SpeechSynthesisLanguage = language;
            using (var synthesizer = new SpeechSynthesizer(config))
            {
                while (true)
                {
                    if (string.IsNullOrEmpty(text))
                    {
                        break;
                    }

                    using (var result = await synthesizer.SpeakTextAsync(text))
                    {
                        if (result.Reason == ResultReason.SynthesizingAudioCompleted)
                        {
                            Console.WriteLine($"Speech synthesized to speaker for text [{text}]");
                        }
                        else if (result.Reason == ResultReason.Canceled)
                        {
                            var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
                            Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                            if (cancellation.Reason == CancellationReason.Error)
                            {
                                Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                                Console.WriteLine($"CANCELED: ErrorDetails=[{cancellation.ErrorDetails}]");
                                Console.WriteLine($"CANCELED: Did you update the subscription info?");
                            }
                        }
                        break;
                    }
                }
            }
        }
        public static async Task RecognitionWithMicrophoneAsync()
        {
            String YourSubscriptionKey = "fed6cde6f90d4535b03975cb77d3d3fc";
            String YourServiceRegion = "eastasia";

            var config = SpeechConfig.FromSubscription(YourSubscriptionKey, YourServiceRegion);

            var language = "zh-TW";

            using (var recognizer = new SpeechRecognizer(config,language))
            {
                Console.WriteLine("Say something...");

                var result = await recognizer.RecognizeOnceAsync().ConfigureAwait(false);

                if (result.Reason == ResultReason.RecognizedSpeech)
                {
                    Console.WriteLine($"RECOGNIZED: Text={result.Text}");
                    text = result.Text;
                }
                else if (result.Reason == ResultReason.NoMatch)
                {
                    Console.WriteLine($"NOMATCH: Speech could not be recognized.");
                }
                else if (result.Reason == ResultReason.Canceled)
                {
                    var cancellation = CancellationDetails.FromResult(result);
                    Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                    if (cancellation.Reason == CancellationReason.Error)
                    {
                        Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                        Console.WriteLine($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
                        Console.WriteLine($"CANCELED: Did you update the subscription info?");
                    }
                }
            }

        }

    }
}


