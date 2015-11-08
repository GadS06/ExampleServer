using System;
using System.Collections.Generic;
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

namespace View
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public ViewModel.ViewModel ViewModel { set; get; }

		public MainWindow()
		{
			ViewModel = new ViewModel.ViewModel();
			DataContext = ViewModel;
			
			InitializeComponent();
		}

		private void CountClick(object sender, RoutedEventArgs e)
		{
			ViewModel.Count();
			PlotView.InvalidatePlot(true);
		}
	}
}
