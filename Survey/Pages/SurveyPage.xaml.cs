using Android.Net.Http;
using Survey.Services;
using System.Data;
using System.Diagnostics;
using static Android.Provider.MediaStore.Audio;

namespace Survey.Pages;

public partial class SurveyPage : ContentPage
{
    private SurveyState _state;
    public SurveyPage(SurveyState state)
    {
        InitializeComponent();
        _state = state;
    }

    private async void GoToMain(object sender, EventArgs e)
    {
        PagesNavigation.GoToAsync(PagesNames.MainPage);
    }

    private async void GoToResults(object sender, EventArgs e)
    {
        PagesNavigation.GoToAsync(PagesNames.ResultsPage);
    }

    private void OnCatOptionChanged(object sender, CheckedChangedEventArgs e)
    {
        try
        {
            if (!e.Value) return;
            RadioButton radio = (RadioButton)sender;
            Border border = radio switch
            {
                PlaningOption => PlaningBorder,
                YesOption => YesBorder,
                LoveOption => LoveBorder,
                _ => throw new Exception("Unknown border")
            };

            ChangeRadio(radio, border);

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }

    private void OnTappedCatOption(object sender, TappedEventArgs e)
    {
        try
        {
            Border border = (Border)sender;
            RadioButton radio = border switch
            {
                PlaningBorder => PlaningOption,
                YesBorder => YesOption,
                LoveBorder => LoveOption,
                _ => throw new Exception("Unknown border")
            };
            ChangeRadio(radio, border);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }

    private void ChangeRadio(Border radio, Border border)
    {
        ResetBorder(YesOption);
        ResetBorder(PlanningOption);
        ResetBorder(LoveOption);


        border.Stroke = Color.FromArgb("#B8573C");
        radio.IsChecked = true;

        _state.CatOwnership = radio == YesOption ? "у вас есть кот или два)"
                                  : radio == PlanningOption ? "вы планируете завести котика)"
                                  : "вы просто любите котиков)";
        HintLabel.Text = radio == YesOption ? "Сколько у вас котиков?"
                              : radio == PlanningOption ? "Сколько планируете котиков?"
                              : "Сколько бы вы хотели котиков?";
    }

    private void ResetBorder(RadioButton radio)
    {
        if (radio.Parent.Parent is Border border)
        {
            border.Stroke = Color.FromArgb("#E7E0D7");
        }
        else
        {
            return;
        }
    }

    private void OnStepperValueChanged(object sender, EventArgs e)
    {
        _state.CatsCount = (int)((Stepper)sender).Value;
    }
}