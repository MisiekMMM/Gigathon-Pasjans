using NAudio.Wave;

namespace Pasjans;

/// <summary>
///  Ta Klasa odpowiada za muzykę
/// </summary>
public class Music
{
    private static string filePath = "music.wav"; // Ścieżka pliku muzycznego
    private static string musicUrl = "https://github.com/MisiekMMM/Files/raw/refs/heads/main/music.wav";
    private static IWavePlayer? waveOut; //Muzyka
    private static AudioFileReader? audioFile;  //plik muzyki
    private static Thread? playbackThread;   // osobny wątek dla muzyki
    private static bool isPlaying = true;


    /// <summary>
    /// Ta metoda włącza muzykę w osobnym wątku
    /// </summary>
    public static async void StartMusic()
    {
        if (!File.Exists(filePath))
        {
            try
            {
                await DownloadFileAsync(musicUrl, "music.wav");
                StartMusic();
            }
            catch (Exception ex)
            {
                Utilities.Blad("Błąd przy pobieraniu muzyki", "spróbuj pobrać plik manualnie i umieść go w folderze z plikiem exe\nhttps://github.com/MisiekMMM/Files/raw/refs/heads/main/music.wav", ex);
            }
        }
        playbackThread = new Thread(() => PlayLoop(filePath));
        playbackThread.IsBackground = true;
        playbackThread.Start();
    }
    static async Task DownloadFileAsync(string url, string destinationPath)
    {
        using (HttpClient client = new HttpClient())
        using (HttpResponseMessage response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
        {
            response.EnsureSuccessStatusCode();

            using (Stream contentStream = await response.Content.ReadAsStreamAsync(),
                        fileStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
            {
                await contentStream.CopyToAsync(fileStream);
            }
        }
    }
    /// <summary>
    /// Ta metoda włącza muzykę i zapętlenie
    /// </summary>
    /// <param name="filePath">Ścieżka do pliku muzycznego</param>
    private static void PlayLoop(string filePath)
    {
        waveOut = new WaveOutEvent();
        audioFile = new AudioFileReader(filePath);
        waveOut.Init(audioFile);
        waveOut.PlaybackStopped += OnPlaybackStopped!;
        waveOut.Volume = (float)(int)Ustawienia.wartosci!["Głośność"] / 100f;
        waveOut.Play();



        // Keep thread alive while playing
        while (isPlaying)
        {
            waveOut.Volume = (float)(int)Ustawienia.wartosci!["Głośność"] / 100f;

            Thread.Sleep(100); // Small delay to reduce CPU usage
        }
    }
    /// <summary>
    /// Uruchamia muzykę ponownie
    /// </summary>
    private static void OnPlaybackStopped(object sender, StoppedEventArgs e)
    {
        if (isPlaying)
        {
            audioFile!.Position = 0;
            waveOut!.Play();
        }
    }
}