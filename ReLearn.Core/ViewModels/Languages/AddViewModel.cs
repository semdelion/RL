﻿using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ReLearn.API.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReLearn.Core.ViewModels.Languages
{
    public class AddViewModel : MvxViewModel
    {
        #region Fields
        #endregion

        #region Commands
        private IMvxAsyncCommand _toDictionaryReplenishment;
        public IMvxAsyncCommand ToDictionaryReplenishment => _toDictionaryReplenishment ?? (_toDictionaryReplenishment = new MvxAsyncCommand(NavigateToDictionaryReplenishment));
        public IMvxAsyncCommand AddWordCommand => new MvxAsyncCommand(AddWord);
        #endregion

        #region Properties       
        private string _word;
        public string Word
        {
            get { return _word; }
            set { SetProperty(ref _word, value); }
        }

        private string _translationWord;
        public string TranslationWord
        {
            get { return _translationWord; }
            set { SetProperty(ref _translationWord, value); }
        }
        #endregion

        #region Services
        protected IMvxNavigationService NavigationService { get; }
        protected IMessageCore Message { get; }
        #endregion

        #region Constructors
        public AddViewModel(IMvxNavigationService navigationService, IMessageCore imessage)
        {
            NavigationService = navigationService;
            Message = imessage;
        }
        #endregion

        #region Private
        private Task<bool> NavigateToDictionaryReplenishment() => NavigationService.Navigate<DictionaryReplenishmentViewModel>();
        private async Task AddWord() //TODO - перевод, async db
        {
            if (Word == "" || Word == null || TranslationWord == null || TranslationWord == "")
                Message.Toast("Resource.String.Enter_word");
            else if (await Task.Run(() => DBWords.WordIsContained(Word.ToLower())))
                Message.Toast("Resource.String.Word_exists");
            else
            {
                await Task.Run(() => DBWords.Insert(Word.ToLower(), TranslationWord.ToLower()));
                Word = TranslationWord = "";
                Message.Toast("Resource.String.Word_Added");
            }
        }
        #endregion

        #region Protected
        #endregion

        #region Public
        public override void ViewCreated()
        {
            base.ViewCreated();
        }
        #endregion
    }
}