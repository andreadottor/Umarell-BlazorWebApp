namespace Dottor.Umarell.Client.Pages;

using Dottor.Umarell.Client.Models;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

public partial class Create
{

    [Required(ErrorMessage = "Il campo 'Titolo' è obbligaorio")]
    [StringLength(250)]
    public string Title { get; set; } = string.Empty;
    [Required(ErrorMessage = "Il campo 'Data inizio' è obbligatorio")]
    public DateTime StartDate { get; set; } = DateTime.Today;
    private IBrowserFile? Image { get; set; }
    public Coordinate Position { get; set; } = Coordinate.Empty;

    private EditContext? EditContext { get; set; } = default!;
    private ValidationMessageStore? MessageStore { get; set; }

    protected override void OnInitialized()
    {
        EditContext = new(this);
        EditContext.OnValidationRequested += HandleValidationRequested;
        MessageStore = new(EditContext);
    }

    private void HandleValidationRequested(object? sender, ValidationRequestedEventArgs args)
    {
        if (MessageStore is not null)
        {
            MessageStore.Clear();

            if (Position == Coordinate.Empty)
                MessageStore.Add(() => Position, "Il campo 'Posizione' è obbligatorio");
        }
    }

    private void LoadFiles(InputFileChangeEventArgs e)
    {
        Image = e.File;
    }

    private async Task Save()
    {
        var model = new BuildingSiteInsertModel();
        model.Title = Title;
        model.Latitude = Position.Latitude;
        model.Longitude = Position.Longitude;
        model.StartDate = StartDate;

        if (Image is not null)
        {
            try
            {
                using Stream stream = Image.OpenReadStream(1024000 * 3);
                using MemoryStream ms = new MemoryStream();
                await stream.CopyToAsync(ms);

                model.FileName = Image.Name;
                model.FileContent = ms.ToArray();
            }
            catch (IOException ex)
            {
                await MessageBoxService.ShowAlertAsync("Error", ex.Message);
                return;
            }
        }
        var result = await ApiProxy.InsertBuildingSiteAsync(model);

        if (result)
        {
            await MessageBoxService.ShowAlertAsync("Segnalazione nuovo cantiere", "Inserimento nuovo cantiere avvenuto con successo");
            Clear();
        }
        else
        {
            await MessageBoxService.ShowAlertAsync("Segnalazione nuovo cantiere", "Errore durante l'inserimento del cantiere.");
        }
    }

    private void Clear()
    {
        Navigation.Refresh(forceReload: true);
    }

}
