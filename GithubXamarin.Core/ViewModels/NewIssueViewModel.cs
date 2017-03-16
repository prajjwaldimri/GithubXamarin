using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Messages;
using GithubXamarin.Core.Utility;
using MvvmCross.Plugins.Messenger;
using Octokit;

namespace GithubXamarin.Core.ViewModels
{
    public class NewIssueViewModel : BaseViewModel
    {
        #region Properties and Commands

        private string _title;
        public string Title
        {
            get { return _title;}
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
            get { return _repositoryId;}
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

        private ItemState _issueItemState;
        public ItemState IssueItemState
        {
            get { return _issueItemState; }
            set
            {
                _issueItemState = IssueItemState;
                RaisePropertyChanged(() => IssueItemState);
            }
        }

        #endregion 

        public NewIssueViewModel(IGithubClientService githubClientService, IMvxMessenger messenger, IDialogService dialogService) : base(githubClientService, messenger, dialogService)
        {
        }

        private async Task CreateIssue()
        {
            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = true });

            if (string.IsNullOrWhiteSpace(Title)) { return; }

            var issuesClient = new IssuesClient(new ApiConnection(GithubClientService.GetAuthorizedGithubClient().Connection));
            var createdIssue = await issuesClient.Create(RepositoryId, new NewIssue(Title) {Body = Body});

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
            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = true });

            if (string.IsNullOrWhiteSpace(Title)) { return; }

            var issuesClient = new IssuesClient(new ApiConnection(GithubClientService.GetAuthorizedGithubClient().Connection));
            var createdIssue = await issuesClient.Update(RepositoryId, IssueNumber, new IssueUpdate()
            {
                Title = Title,
                Body = Body,
                State = IssueItemState
            });

            await issuesClient.Labels.AddToIssue(RepositoryId, createdIssue.Number,
                StringToStringArrayConverter.Convert(Labels));

            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = false });

            ShowViewModel<IssueViewModel>(new
            {
                issueNumber = createdIssue.Number,
                repositoryId = RepositoryId,
            });
        }
    }
}
