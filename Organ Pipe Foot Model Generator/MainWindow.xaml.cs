﻿using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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

    private void btnSaveFile_Click(object sender, RoutedEventArgs e)
    {
        var logic = new BtnSaveLogic();
        logic.SaveFile();
    }

    private void btnCreateSquare_Click(object sender, RoutedEventArgs e)
    {
        string txbText = txbLength.Text;
        int length = int.Parse(txbText);

        var logic = new btnCreateSquareLogic();
        logic.CreateSquareModel(length);
    }

    private void btnRead_Click(object sender, RoutedEventArgs e)
    {
        var Logic = new btnReadLogic();
        Logic.ReadFile();
    }
}