using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace APM_Meter.Controllers
{
    class InputController
    {
        public InputController() { }

        public long totalInput = 0;
        public long apmInput = 0;
        public long apsInput = 0;

        private long currentMinuteInput = 0;
        private long currentSecondInput = 0;

        /// <summary>
        /// Increments input number based on mouse/key events
        /// </summary>
        public void IncrementInput()
        {
            totalInput += 1;
            currentMinuteInput += 1;
            currentSecondInput += 1;
        }

        /// <summary>
        /// Run the repeating task to calculate APM every minutes
        /// </summary>
        public void RunAPM()
        {
            CancellationTokenSource ctx = new CancellationTokenSource();
            TimeSpan timeSpan = TimeSpan.FromSeconds(60);
            RepeatingTask(CalculAPM, timeSpan, ctx.Token);
        }

        /// <summary>
        /// Run the repeating task to calculate APS every seconds
        /// </summary>
        public void RunAPS()
        {
            CancellationTokenSource ctx = new CancellationTokenSource();
            TimeSpan timeSpan = TimeSpan.FromSeconds(1);
            RepeatingTask(CalculAPS, timeSpan, ctx.Token);
        }

        /// <summary>
        /// Calcul the APM
        /// </summary>
        private void CalculAPM()
        {
            apmInput = currentMinuteInput;
            currentMinuteInput = 0;
        }

        /// <summary>
        /// Calcul the APS
        /// </summary>
        private void CalculAPS()
        {
            apsInput = currentSecondInput;
            currentSecondInput = 0;
        }

        /// <summary>
        /// Repeating a task based on a period time
        /// </summary>
        /// <param name="action"></param>
        /// <param name="period"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private static async Task RepeatingTask(Action action, TimeSpan period, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(period, cancellationToken);

                if (!cancellationToken.IsCancellationRequested)
                    action();
            }
        }
    }
}
