using System.Collections.Concurrent;
using System.Text;
using System.Threading.Channels;

namespace Lab2
{
    public class Program
    {
        public static async Task Main()
        {
            var inputChannel = Channel.CreateUnbounded<string>();
            var outputChannel = Channel.CreateUnbounded<string>();

            _ = RedirectWithTimeout(inputChannel.Reader, outputChannel.Writer, 200);

            await inputChannel.Writer.WriteAsync("Test1");

            Console.WriteLine(await outputChannel.Reader.ReadAsync());

            await inputChannel.Writer.WriteAsync("Test2");
            Console.WriteLine(await outputChannel.Reader.ReadAsync());

            await Task.Delay(600);
            await inputChannel.Writer.WriteAsync("Test3");
            Console.WriteLine(await outputChannel.Reader.ReadAsync());
        }

        public static async Task RedirectWithTimeout(ChannelReader<string> input, ChannelWriter<string> output, int timeout)
        {
            while (true)
            {
                var cts = new CancellationTokenSource(timeout);
                try
                {
                    var item = await input.ReadAsync(cts.Token);
                    await output.WriteAsync(item);
                }
                catch (OperationCanceledException)
                {
                    await output.WriteAsync(":timeout");
                }
            }
        }
    }
}