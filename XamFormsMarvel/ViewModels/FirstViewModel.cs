
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Threading.Tasks;
using XamFormsMarvel.Services;
using Xamarin.Forms;
using System;

namespace XamFormsMarvel.ViewModels
{
    public class FirstViewModel : BaseViewModel
    {
		private readonly IMarvelApiService _marvelService;

		public FirstViewModel (IMarvelApiService marvelService = null)
		{
			_marvelService = marvelService ?? DependencyService.Get<IMarvelApiService>();

		}

		public async Task Init()
		{
			await LoadData();
		}

		#region SearchText

		private string _SearchText;

		public string SearchText {
			get {
				return _SearchText;
			}
			set {
				_SearchText = value;
				RaisePropertyChanged();
			}
		}

		#endregion

		#region CharacterList

		private List<CharacterItemViewModel> _CharacterList;

		public List<CharacterItemViewModel> CharacterList {
			get {
				return _CharacterList;
			}
			set {
				_CharacterList = value;
				RaisePropertyChanged();
			}
		}

		#endregion

		#region SearchByName Command

		private ICommand _SearchByName;

		public ICommand SearchByName {
			get {
				return _SearchByName ?? (_SearchByName = new Command (
					async () => await ExecuteSearchByNameCommand ())); 
			}
		}

		private async Task ExecuteSearchByNameCommand ()
		{
			await LoadData (SearchText);
		}

		//bool CanExecuteSearchByNameCommand()
		//{
		//	// Condición para poder ejecutar el commando
		//	return SearchText?.Length > 0;
		//}

		#endregion

		#region LoadData

		private async Task LoadData (string filter = null, int limit = 0, int offset = 0)
		{
			IsBusy = true;

			var result = await _marvelService.GetCharacters (filter, limit, offset);


			if (result != null) {
				CharacterList = (from p in result.Results
				                 select new CharacterItemViewModel () {
						Id = p.Id,
						Name = p.Name,
						Thumbnail = p.Thumbnail.Path + "." + p.Thumbnail.Extension,
						Description = p.Description
				}).ToList ();
			}

			IsBusy = false;

		}

		#endregion
    }
}
