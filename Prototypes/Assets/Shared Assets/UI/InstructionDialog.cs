using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;
using System.Collections.Generic;

namespace DSS.UI
{
	public class InstructionDialog : MonoBehaviour
	{
		public class DialogResponse
		{
			private Action _okAction;
			private Action _cancelAction;

			public DialogResponse Ok(Action okAction)
			{
				_okAction += okAction;
				return this;
			}
			
			public DialogResponse Cancel(Action cancelAction)
			{
				_cancelAction += cancelAction;
				return this;
			}

			internal void OkPressed()
			{
				if (_okAction != null) 
				{
					_okAction();
				}
			}

			internal void CancelPressed()
			{
				if (_cancelAction != null) 
				{
					_cancelAction();
				}
			}
		}

		public static DialogResponse Say(string message, bool showCancel = true)
		{
			return Say(message, Color.clear, showCancel);
		}

		public static DialogResponse Say(string message, Color highlight, bool showCancel = true)
		{
			if (!s_dialog) 
			{
				throw new InvalidOperationException("Cannot create a dialog until after InstructionDialog.Awake() has happened");
			}
			return s_dialog.ShowDialog (message, showCancel, highlight);
		}

		private static InstructionDialog s_dialog;
		private DialogResponse _currentDialog;

		[SerializeField]
		private Text _message;
		[SerializeField]
		private GameObject _cancelButton;
		[SerializeField]
		private Image _highlight;


		void Awake()
		{
			s_dialog = this;
			HideDialog();
		}

		void OnDestroy()
		{
			s_dialog = null;
		}

		private void HideDialog()
		{
			_currentDialog = null;
			gameObject.SetActive(false);
		}

		internal DialogResponse ShowDialog(string message, bool showCancel, Color highlight)
		{
			gameObject.SetActive (true);

			_message.text = message;
			if (!showCancel) 
			{
				_cancelButton.SetActive(false);
			}

			_highlight.color = highlight;

			_currentDialog = new DialogResponse ();
			return _currentDialog
				.Ok(HideDialog)
				.Cancel (HideDialog);
		}

		public void OkPressed()
		{
			if (_currentDialog == null) 
			{
				throw new InvalidOperationException("No current dialog");
			}
			_currentDialog.OkPressed ();
		}

		public void CancelPressed()
		{
			if (_currentDialog == null) 
			{
				throw new InvalidOperationException("No current dialog");
			}
			_currentDialog.CancelPressed ();

		}
	}
}