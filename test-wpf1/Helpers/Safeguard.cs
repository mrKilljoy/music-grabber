using System;
using System.Threading.Tasks;

namespace test_wpf1.Helpers
{
    public class Safeguard
    {
        public static void TryRun(Action action, bool terminateOnFailure = false)
        {
            try
            {
                action();
            }
            catch (AggregateException aex)
            {
                //  create and show a single error message
            }
            catch (Exception ex)
            {
                ErrorHelper.ShowError(ex.Message);

                if (terminateOnFailure)
                {
                    Environment.Exit(1);
                }
            }
        }

        public static async Task TryRunAsync(Func<Task> func)
        {
            try
            {
                await func();
            }
            catch(AggregateException aex)
            {
                //  create and show a single error message
            }
            catch (Exception ex)
            {
                ErrorHelper.ShowError(ex.Message);
            }
        }
    }
}
