using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Threading;

namespace Naklih.Com.Pomodoro
{
    public class EditableComboBox : ComboBox
    {
        TextBox _textBox;
               
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            
            Dispatcher.BeginInvoke(new Action(() => setupTextBox(false)), DispatcherPriority.ContextIdle, null);
        }


        private void setupTextBox(bool focus)
        {
            _textBox = Template.FindName("PART_EditableTextBox", this) as TextBox;
            if (_textBox != null)
            {
                _textBox.GotKeyboardFocus += _textBox_GotFocus;
                this.Unloaded += EditableComboBox_Unloaded;
                if(focus)
                {
                    _textBox.Focus();
                }
            }
        }

        public TextBox TextBoxControl
        {
            get
            {
                return _textBox;
            }
        }

        public new bool IsEditable
        {
            get
            {
                return base.IsEditable;
            }
            set
            {
                base.IsEditable = value;
                if(value == false)
                {
                    detatchTexboxEvents();
                }
            }
        }

        public void ActivateEditMode()
        {
            base.IsEditable = true;
            Dispatcher.BeginInvoke(new Action(() => setupTextBox(true)), DispatcherPriority.ContextIdle, null);
            
        }

        public void DeactivateEditMode()
        {
            base.IsEditable = true;
            detatchTexboxEvents();

        }

        private void detatchTexboxEvents()
        {
            if(_textBox != null)
            {
                
                _textBox.GotKeyboardFocus -= _textBox_GotFocus;
                
            }
            
            this.Unloaded -= EditableComboBox_Unloaded;
        }

        void EditableComboBox_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            detatchTexboxEvents();
        }

        void _textBox_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            _textBox.Select(_textBox.Text.Length, 0); // set caret to end of text
        }


        public bool HasSelectedAnItem
        {
            get
            {
                return this.Text.Length > 0;
            }
        }
    }
}
