﻿using SteamStorage.Models;
using SteamStorage.Utilities.Dialog;
using SteamStorage.ViewModels.BaseViewModels;

namespace SteamStorage.ViewModels;

public class MessageDialogViewModel : BaseDialogViewModel
{
    #region Fields

    private readonly MessageDialogModel _messageDialogModel;

    #endregion Fields

    #region Properties

    public string Message
    {
        get => _messageDialogModel.Message;
    }

    public DialogUtility.MessageType SelectedMessageType
    {
        get => _messageDialogModel.SelectedMessageType;
    }

    public DialogUtility.MessageButtons SelectedMessageButtons
    {
        get => _messageDialogModel.SelectedMessageButtons;
    }
    
    public bool IsInfoVisible
    {
        get => _messageDialogModel.IsInfoVisible;
    }
    
    public bool IsErrorVisible
    {
        get => _messageDialogModel.IsErrorVisible;
    }
    
    public bool IsQuestionVisible
    {
        get => _messageDialogModel.IsQuestionVisible;
    }
    
    public bool IsOkVisible
    {
        get => _messageDialogModel.IsOkVisible;
    }
    
    public bool IsSaveVisible
    {
        get => _messageDialogModel.IsSaveVisible;
    }
    
    public bool IsCancelVisible
    {
        get => _messageDialogModel.IsCancelVisible;
    }

    #endregion Properties

    #region Constructor

    public MessageDialogViewModel(
        MessageDialogModel messageDialogModel) : base(messageDialogModel)
    {
        _messageDialogModel = messageDialogModel;
        messageDialogModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor

    #region Methods

    public void SetMessageBox(string message,
        DialogUtility.MessageType messageType,
        DialogUtility.MessageButtons messageButtons)
    {
        _messageDialogModel.SetMessageBox(message, messageType, messageButtons);
    }

    #endregion Methods
}
