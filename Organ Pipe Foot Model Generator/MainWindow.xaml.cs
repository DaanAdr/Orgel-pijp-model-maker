using System.Windows;
using ACadSharp.Entities;
using ACadSharp.IO;
using ACadSharp;
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

        PipeFootTemplate pipeFootTemplate = new PipeFootTemplate(100, 100, topDiameter, bottomDiameter, height);
        PipeFootMeasurements pipeFootMeasurements = pipeFootTemplate.Measurements;

        //Display calculated measurements
        txbLengthSlantedSide.Text = pipeFootMeasurements.LengthSlantedSide.ToString();
        txbLengthInnerDiameter.Text = pipeFootMeasurements.LengthInnerDiameter.ToString();
        txbLengthOuterDiameter.Text = pipeFootMeasurements.LengthOuterDiameter.ToString();
        txbSmallRadius.Text = pipeFootMeasurements.SmallRadius.ToString();
        txbLargeRadius.Text = pipeFootMeasurements.LargeRadius.ToString();
        txbCornerDegrees.Text = pipeFootMeasurements.CornerInDegrees.ToString();

        //Save file for model
        string filePath = string.Empty;

        //Create a file to put the square in
        SaveFileDialog saveFileDialog = new SaveFileDialog
        {
            Filter = "DXF files (*.dxf)|*.dxf|All files (*.*)|*.*",
            Title = "Save a DXF File",
            FileName = "PipeFootModel.dxf" // Default file name
        };

        if ((bool)saveFileDialog.ShowDialog())
        {
            filePath = saveFileDialog.FileName; // Return the selected file path
        }

        //Add the insert into a document
        CadDocument doc = new CadDocument();

        //Add lines directly to the document
        doc.Entities.Add(pipeFootTemplate.Bottomline);
        doc.Entities.Add(pipeFootTemplate.SmallArc);
        //doc.Entities.Add(leftLine);
        //doc.Entities.Add(rightLine);

        // Save the document using DxfWriter

        using (DxfWriter writer = new DxfWriter(filePath, doc, false))
        {
            writer.Write();
        }
    }
}