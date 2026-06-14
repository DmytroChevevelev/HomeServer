using SmartHome.Simulator.Runtime;

namespace SmartHome.Simulator.Commands;

public sealed class RegisterDeviceCommandHandler(SimulatorBackendClient backendClient)
{
    public async Task<bool> ExecuteAsync(SimulatorDeviceProfile profile, TextWriter output, CancellationToken ct)
    {
        try
        {
            using var response = await backendClient.RegisterDeviceAsync(profile, ct);
            var body = await response.Content.ReadAsStringAsync(ct);

            if (response.IsSuccessStatusCode)
            {
                await output.WriteLineAsync($"register-device: success ({(int)response.StatusCode})");
                if (!string.IsNullOrWhiteSpace(body))
                {
                    await output.WriteLineAsync(body);
                }

                return true;
            }

            await output.WriteLineAsync($"register-device: failed ({(int)response.StatusCode})");
            if (!string.IsNullOrWhiteSpace(body))
            {
                await output.WriteLineAsync(body);
            }

            return false;
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            await output.WriteLineAsync($"register-device: failed to reach backend ({ex.Message})");
            return false;
        }
    }
}