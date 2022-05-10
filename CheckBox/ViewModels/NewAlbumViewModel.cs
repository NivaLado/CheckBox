using CheckBox.Models;
using System;
using Xamarin.Forms;

namespace CheckBox.ViewModels
{
    public class NewAlbumViewModel : BaseViewModel
    {
        private string name;
        private string description;

        public NewAlbumViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return !string.IsNullOrWhiteSpace(name)
                && !string.IsNullOrWhiteSpace(description);
        }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            Album newAlbum = new Album()
            {
                Id = Guid.NewGuid().ToString(),
                Name = Name,
                Description = Description
            };

            // Gather Id from result
            // await DataStore.AddItemAsync(newItem);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
