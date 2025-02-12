using System.Windows;
using Microsoft.Win32;
using Organ_Pipe_Foot_Model_Generator.Logic;

namespace Organ_Pipe_Foot_Model_Generator;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void btnCreateSquare_Click(object sender, RoutedEventArgs e)
    {
        string filePath = string.Empty;

        //Create a file to put the square in
        SaveFileDialog saveFileDialog = new SaveFileDialog
        {
            Filter = "DXF files (*.dxf)|*.dxf|All files (*.*)|*.*",
            Title = "Save a DXF File",
            FileName = "SquareModel.dxf" // Default file name
        };

        if ((bool)saveFileDialog.ShowDialog())
        {
            filePath = saveFileDialog.FileName; // Return the selected file path
        }


        string txbText = txbLength.Text;
        int length = int.Parse(txbText);

        var logic = new btnCreateSquareLogic();
        logic.CreateSquareModel(length, filePath);
    }

    private void btnRead_Click(object sender, RoutedEventArgs e)
    {
        var Logic = new btnReadLogic();
        Logic.ReadFile();
    }
}