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

            await WriteAndPrint(inputChannel.Writer, outputChannel.Reader, "Test1");
            await WriteAndPrint(inputChannel.Writer, outputChannel.Reader, "Test2");

            await Task.Delay(600);
            await WriteAndPrint(inputChannel.Writer, outputChannel.Reader, "Test3");
        }

        public static async Task WriteAndPrint(ChannelWriter<string> writer, ChannelReader<string> reader, string message)
        {
            await writer.WriteAsync(message);
            Console.WriteLine(await reader.ReadAsync());
        }

        public static async Task RedirectWithTimeout(ChannelReader<string> input, ChannelWriter<string> output, int timeout)
        {
            while (true)
            {
                try
                {
                    var item = await ReadWithTimeout(input, new CancellationTokenSource(timeout).Token);
                    await Write(output, item);
                }
                catch (OperationCanceledException)
                {
                    await Write(output, "timeout");
                }
            }
        }

        public static async Task<string> ReadWithTimeout(ChannelReader<string> reader, CancellationToken token)
        {
            return await reader.ReadAsync(token);
        }

        public static async Task Write(ChannelWriter<string> writer, string message)
        {
            await writer.WriteAsync(message);
        }
    }
}