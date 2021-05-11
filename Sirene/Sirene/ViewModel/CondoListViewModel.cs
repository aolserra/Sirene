using Firebase.Database;
using Sirene.Models;
using Sirene.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sirene.ViewModel
{
    public class CondoListViewModel : INotifyPropertyChanged
    {
        List<Comunidade> Condos { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand SelectCommand { get; set; }

        public CondoListViewModel()
        {
            ListingCondos();
            SelectCommand = new Command(SelectCmd);
        }

        private async void ListingCondos()
        {
            Condos = await ComunidadeService.GetInstance().GetComunidadeList(null, null);
            CondoList = new ObservableCollection<string>();
            foreach (Comunidade condo in Condos)
            {
                CondoList.Add(condo.NomeComunidade);
            }
        }

        private void SelectCmd()
        {
            if (!string.IsNullOrEmpty(Condo))
            {
                SelectedCondo = Condo;
            }
            else
            {
                SelectedCondo = "Selecione um Condomínio";
            }
        }

        //ObservableCollection<string> condoList;
        ObservableCollection<string> condoList;
        public ObservableCollection<string> CondoList
        {
            get
            {
                return condoList;
            }
            set
            {
                condoList = value;
                OnPropertyChanged();
            }
        }

        string _condo;
        public string Condo
        {
            get
            {
                return _condo;
            }
            set
            {
                _condo = value;
                OnPropertyChanged();
            }
        }

        string _selectedCondo;
        public string SelectedCondo
        {
            get
            {
                return _selectedCondo;
            }
            set
            {
                _selectedCondo = value;
                OnPropertyChanged();
            }
        }
    }
}

