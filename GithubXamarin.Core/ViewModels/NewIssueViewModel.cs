﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Messages;
using GithubXamarin.Core.Utility;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using Octokit;

namespace GithubXamarin.Core.ViewModels
{
    public class NewIssueViewModel : BaseViewModel
    {
        #region Properties and Commands

        private ICommand _submitCommand;
        public ICommand SubmitCommand
        {
            get {
                _submitCommand = _submitCommand ?? new MvxAsyncCommand(CreateOrUpdateIssue);
                return _submitCommand;
            }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        private string _body;
        public string Body
        {
            get { return _body; }
            set
            {
                _body = value;
                RaisePropertyChanged(() => Body);
            }
        }

        private string _labels;
        public string Labels
        {
            get { return _labels; }
            set
            {
                _labels = value;
                RaisePropertyChanged(() => Labels);
            }
        }

        private int _repositoryId;
        public int RepositoryId
        {
            get { return _repositoryId; }
            set
            {
                _repositoryId = value;
                RaisePropertyChanged(() => RepositoryId);
            }
        }

        private int _issueNumber;
        public int IssueNumber
        {
            get { return _issueNumber; }
            set
            {
                _issueNumber = value;
                RaisePropertyChanged(() => IssueNumber);
            }
        }

        private int _issueStateSelectedIndex;
        public int IssueStateSelectedIndex
        {
            get { return _issueStateSelectedIndex; }
            set
            {
                _issueStateSelectedIndex = value;
                switch (value)
                {
                    case 0:
                        IssueItemState = ItemState.Open;
                        break;
                    case 1:
                        IssueItemState = ItemState.Closed;
                        break;
                }
                RaisePropertyChanged(() => IssueStateSelectedIndex);
            }
        }

        private ItemState _issueItemState;
        public ItemState IssueItemState
        {
            get { return _issueItemState; }
            set
            {
                _issueItemState = value;
                RaisePropertyChanged(() => IssueItemState);
            }
        }

        private bool _isEdit;
        public bool IsEdit
        {
            get { return _isEdit; }
            set
            {
                _isEdit = value;
                RaisePropertyChanged(() => IsEdit);
            }
        }

        #endregion 

        public NewIssueViewModel(IGithubClientService githubClientService, IMvxMessenger messenger, IDialogService dialogService) : base(githubClientService, messenger, dialogService)
        {
        }

        public async void Init(int repositoryId, int issueNumber, string issueTitle = null, string issueBody = null, string labels = null)
        {
            RepositoryId = repositoryId;
            IsEdit = false;
            IssueNumber = issueNumber;
            if (!(string.IsNullOrWhiteSpace(issueTitle)))
            {
                Title = issueTitle;
                Body = issueBody;
                Labels = labels;
                IsEdit = true;
            }
        }

        private async Task CreateOrUpdateIssue()
        {
            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = true });

            if (IsEdit)
            {
                await UpdateIssue();
            }
            else
            {
                await CreateIssue();
            }

            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = false });
        }


        private async Task CreateIssue()
        {
            if (!IsInternetAvailable())
            {
                await DialogService.ShowDialogASync("I am a very PC person! I don't like working on laptops", "Internet not available");
                return;
            }

            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = true });

            if (string.IsNullOrWhiteSpace(Title)) { return; }

            var issuesClient = new IssuesClient(new ApiConnection(GithubClientService.GetAuthorizedGithubClient().Connection));
            var createdIssue = await issuesClient.Create(RepositoryId, new NewIssue(Title) { Body = Body });

            await issuesClient.Labels.AddToIssue(RepositoryId, createdIssue.Number,
                StringToStringArrayConverter.Convert(Labels));

            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = false });

            ShowViewModel<IssueViewModel>(new
            {
                issueNumber = createdIssue.Number,
                repositoryId = RepositoryId,
            });
        }

        private async Task UpdateIssue()
        {
            if (!IsInternetAvailable())
            {
                await DialogService.ShowDialogASync("No internet, No work :(", "Can't submit update");
                return;
            }

            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = true });

            if (string.IsNullOrWhiteSpace(Title) || IssueNumber == null) { return; }

            var issuesClient = new IssuesClient(new ApiConnection(GithubClientService.GetAuthorizedGithubClient().Connection));
            var createdIssue = await issuesClient.Update(RepositoryId, IssueNumber, new IssueUpdate()
            {
                Title = Title,
                Body = Body,
                State = IssueItemState
            });

            if (!(string.IsNullOrWhiteSpace(Labels)))
            {
                await issuesClient.Labels.AddToIssue(RepositoryId, createdIssue.Number,
                    StringToStringArrayConverter.Convert(Labels));
            }

            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = false });

            ShowViewModel<IssueViewModel>(new
            {
                issueNumber = createdIssue.Number,
                repositoryId = RepositoryId,
            });
        }
    }
}