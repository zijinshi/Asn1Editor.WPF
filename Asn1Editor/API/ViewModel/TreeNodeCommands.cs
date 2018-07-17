﻿using System;
using System.Windows.Input;
using SysadminsLV.Asn1Editor.API.Interfaces;
using SysadminsLV.Asn1Editor.API.ModelObjects;
using SysadminsLV.Asn1Editor.API.Utils;
using SysadminsLV.WPF.OfficeTheme.Toolkit.Commands;

namespace SysadminsLV.Asn1Editor.API.ViewModel {
    class TreeNodeCommands : ITreeCommands {
        readonly IWindowFactory _windowFactory;
        readonly IDataSource _data;

        public TreeNodeCommands(IWindowFactory windowFactory, IDataSource data) {
            _windowFactory = windowFactory;
            _data = data;
            ShowNodeTextViewer = new RelayCommand(showNodeTextViewer, canExecuteTreeCommands);
            EditNodeCommand = new RelayCommand(editNodeContent, canExecuteTreeCommands);
            AddNewNodeCommand = new RelayCommand(addNewNode, canAddNewNode);
        }

        public ICommand ShowNodeTextViewer { get; }
        public ICommand EditNodeCommand { get; }
        public ICommand SaveNodeCommand { get; }
        public ICommand AddNewNodeCommand { get; }
        public ICommand DeleteNodeCommand { get; }
        public ICommand CutNodeCommand { get; }
        public ICommand CopyNodeCommand { get; }
        public ICommand PasteBeforeCommand { get; }
        public ICommand PasteAfterCommand { get; }
        public ICommand PasteLastCommand { get; }

        void showNodeTextViewer(Object o) {
            _windowFactory.ShowNodeTextViewer();
        }
        void editNodeContent(Object o) {
            _windowFactory.ShowNodeContentEditor((NodeEditMode)o);
        }
        void addNewNode(Object o) {
            Asn1Lite nodeValue = _windowFactory.ShowNodeContentEditor(NodeEditMode.NewNode);
            if (nodeValue == null) { return; }
            if (_data.Tree.Count == 0) {
                // add new root node
                Asn1TreeNode node = new Asn1TreeNode(nodeValue);
                _data.Tree.Add(node);
                _data.FinishBinaryUpdate();
            } else {
                _data.SelectedNode.AddChild(nodeValue, true);
                _data.FinishBinaryUpdate();
            }
        }
        Boolean canExecuteTreeCommands(Object o) {
            return _data.SelectedNode != null;
        }
        Boolean canAddNewNode(Object o) {
            return _data.Tree.Count == 0
                   || _data.SelectedNode != null;
        }
    }
}
