using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace felkaruRablo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
		private int balance = 100; // kezdő kredit - átállítható
		private const int spinCost = 10;

		private Random rnd = new Random();

		private string[] symbols;

		private DispatcherTimer spinTimer;
		private int tick;
		private bool isSpinning = false;

		private string r1, r2, r3;

		public MainWindow()
        {
            InitializeComponent();

			// teszt 
			int symbolCount = 6; // szimbólumok száma - átállítható

			symbols = new string[symbolCount];

			for (int i = 0; i < symbolCount; i++)
			{
				symbols[i] = $"Images/symbol{(i + 1).ToString("D2")}.png";
			}

			UpdateBalance();

			// anim. időzítő
			spinTimer = new DispatcherTimer();
			spinTimer.Interval = TimeSpan.FromMilliseconds(100);
			spinTimer.Tick += SpinAnimation;
		}

		private void Spin_Click(object sender, RoutedEventArgs e)
		{
			if (isSpinning) return;

			if (balance < spinCost)
			{
				ResultText.Text = "Not enough credit!";
				return;
			}

			balance -= spinCost;
			UpdateBalance();

			ResultText.Text = "";
			isSpinning = true;

			tick = 0;
			spinTimer.Start();
		}

		private void SpinAnimation(object sender, EventArgs e)
		{
			tick++;

			Reel1.Source = RandomSymbol();
			Reel2.Source = RandomSymbol();
			Reel3.Source = RandomSymbol();

			if (tick > 20)
			{
				spinTimer.Stop();
				FinishSpin();
			}
		}

		private void FinishSpin()
		{
			// eredmény
			r1 = RandomSymbolPath();
			r2 = RandomSymbolPath();
			r3 = RandomSymbolPath();

			Reel1.Source = new BitmapImage(new Uri(r1, UriKind.Relative));
			Reel2.Source = new BitmapImage(new Uri(r2, UriKind.Relative));
			Reel3.Source = new BitmapImage(new Uri(r3, UriKind.Relative));

			CheckWin();

			isSpinning = false;
		}

		private BitmapImage RandomSymbol()
		{
			return new BitmapImage(new Uri(RandomSymbolPath(), UriKind.Relative));
		}

		private string RandomSymbolPath()
		{
			return symbols[rnd.Next(symbols.Length)];
		}

		private void CheckWin()
		{
			if (r1 == r2 && r2 == r3)
			{
				balance += 50;
				ResultText.Text = "BIG WIN! (+50 credit)";
			}
			else if (r1 == r2 || r2 == r3 || r1 == r3)
			{
				balance += 20;
				ResultText.Text = "You win! (+20 credit)";
			}
			else
			{
				ResultText.Text = "Lose!";
			}

			UpdateBalance();
		}

		private void UpdateBalance()
		{
			BalanceText.Text = $"Balance: {balance}";
		}

	}
}