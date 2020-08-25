using APM_Meter.Hooks;
using APM_Meter.Controllers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Windows.Threading;

namespace APM_Meter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private InputController inputController;
        private GlobalMouseHook globalMouseHook;
        private GlobalKeyboardHook globalKeyboardHook;

        public MainWindow()
        {
            InitializeComponent();
            SetupInputHook();
            SetupInputController();
            SetupTimer();

            Label.MouseLeftButtonDown += Label_MouseLeftButtonDown;
            Closing += MainWindow_Closing;
        }

        /// <summary>
        /// Setup Global Hooks
        /// </summary>
        private void SetupInputHook()
        {
            globalKeyboardHook = new GlobalKeyboardHook();
            globalKeyboardHook.KeyboardPressed += OnKeyPressed;

            globalMouseHook = new GlobalMouseHook();
            globalMouseHook.Start();
            globalMouseHook.MouseAction += OnMouseAction;
        }

        /// <summary>
        /// Setup Input Controller to calculate APM and APS
        /// </summary>
        private void SetupInputController()
        {
            inputController = new InputController();
            inputController.RunAPM();
            inputController.RunAPS();
        }

        /// <summary>
        /// Timer is used to update UI
        /// </summary>
        private void SetupTimer()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.5);
            timer.Tick += TimerTick;
            timer.Start();
        }

        /// <summary>
        /// Callback when the tick is reached
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerTick(object sender, EventArgs e)
        {
            UpdateTextBlock();
        }

        /// <summary>
        /// Update UI components
        /// </summary>
        private void UpdateTextBlock()
        {
            Total.Text = "Total : " + inputController.totalInput;
            APM.Text = inputController.apmInput + "/m";
            APS.Text = inputController.apsInput + "/s";
        }

        /// <summary>
        /// Performs application-defined tasks associated 
        /// with freeing, releasing, or resetting unmanaged ressources;
        /// </summary>
        public void Dispose()
        {
            globalKeyboardHook?.Dispose();
        }

        /// <summary>
        /// Callback when a mouse left button down is captured on Label
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        /// <summary>
        /// Callback when a key event is captured by the hook
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyPressed(object sender, GlobalKeyboardHookEventArgs e)
        {
            //Debug.WriteLine(e.KeyboardData.VirtualCode);

            //if (e.KeyboardData.VirtualCode != GlobalKeyboardHook.VkSnapshot)
            //    return;

            if (e.KeyboardData.VirtualCode == 92)
                return;

            if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyUp)
            {
                inputController.IncrementInput();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Callback when a mouse event is captured by the hook
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseAction(object sender, EventArgs e)
        {
            inputController.IncrementInput();
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Dispose();
        }
    }
}
