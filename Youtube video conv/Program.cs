using System;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;
using System.IO;

namespace Youtube
{
    class Program
    {
        static async Task Main(string[] args) {

            await YTDownload();
        }

        static async Task YTDownload()
        {
            
            var youtube = new YoutubeClient();

            // You can specify either the video URL or its ID
            var videoUrl = "https://www.youtube.com/watch?v=D8kcwWvPVZQ";
            var video = await youtube.Videos.GetAsync(videoUrl);
            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(videoUrl);

            var streamInfo = streamManifest.GetMuxedStreams().GetWithHighestVideoQuality();


            var streamAudioInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();


            var title = video.Title; 
            var author = video.Author.ChannelTitle; 
            var duration = video.Duration; 


            // Create a new file stream for the mp4 file
            //var filePath = Path.Combine(videosDirectory, $"{title}.mp4");
            string filePath = @$"D:\Andrei\{title}.mp3";
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                // Open a stream to the video URL
                using (var videoStream = await youtube.Videos.Streams.GetAsync(streamAudioInfo))
                {
                    // Write the stream data to the file stream
                    await videoStream.CopyToAsync(fileStream);
                }
            }

            Console.WriteLine($"Title: {title}");
            Console.WriteLine($"Author: {author}");
            Console.WriteLine($"Duration: {duration}");
            Console.WriteLine($"Saved video to: {filePath}");
        }
    }
}