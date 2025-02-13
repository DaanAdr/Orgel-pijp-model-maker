using System.Windows;
using Microsoft.Win32;
using Organ_Pipe_Foot_Model_Generator.Entities;
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

    #region Prevent input from being non numeric
    private void txbTopDiameter_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        e.Handled = !InputValidation.InputIsNumericOnly(e.Text);
    }

    private void txbBottomDiameter_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        e.Handled = !InputValidation.InputIsNumericOnly(e.Text);
    }

    private void txbHeight_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        e.Handled = !InputValidation.InputIsNumericOnly(e.Text);
    }
    #endregion

    private void btnCalculatePipeFootMeasurements_Click(object sender, RoutedEventArgs e)
    {
        double topDiameter = double.Parse(txbTopDiameter.Text);
        double bottomDiameter = double.Parse(txbBottomDiameter.Text);
        double height = double.Parse(txbHeight.Text);

        PipeFootMeasurements pipeFootMeasurements = new PipeFootMeasurements(topDiameter, bottomDiameter, height);
        pipeFootMeasurements.SetCalculatedMeasurements();

        //Display calculated measurements
        txbLengthSlantedSide.Text = pipeFootMeasurements.LengthSlantedSide.ToString();
    }
}