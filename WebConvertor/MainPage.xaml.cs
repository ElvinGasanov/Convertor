using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebConvertor
{
    public class ViewModel : BindableObject
    {
        
        private string _convertedValue;

        private string _pickerFrom;

        private string _pickerTo;

        private float _entryFrom;

        private DateTime selectedDate;

        private Parser _xmlFileParser;
        private DateTime _datePicker;

        public ViewModel()
        {
            MDatePicker = DateTime.Now;

            PropertyChanged += ViewModel_PropertyChanged;

        }

        private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            OnGetExchangeRatesClicked();
        }

        public string MConvertedValue
        {
            get { return _convertedValue; }
            set
            {
                if(_convertedValue != value)
                {
                    _convertedValue = value;
                    OnPropertyChanged(nameof(MConvertedValue));
                    Debug.WriteLine("ConvertevValue = " + _convertedValue);
                }
            }
        }

        public string MPickerFrom
        {
            get { return _pickerFrom; }
            set
            {
                if(_pickerFrom != value)
                {
                    _pickerFrom = value;
                    OnPropertyChanged(nameof(MPickerFrom));
                    Debug.WriteLine("PickerFrom = " + _pickerFrom);
                }
            }
        }

        public string MPickerTo
        {
            get { return _pickerTo; }
            set
            {
                if( _pickerTo != value)
                {
                    _pickerTo = value;
                    OnPropertyChanged(nameof(MPickerTo));
                    Debug.WriteLine("PickerTo = " + _pickerTo);
                }
            }
        }

        public float MEntryFrom
        {
            get { return _entryFrom; }
            set
            {
                if(  value != _entryFrom )
                {
                    _entryFrom = value;
                    OnPropertyChanged(nameof(MEntryFrom));
                    Debug.WriteLine("EntryFrom = " + _entryFrom);
                }
            }
        }

        public DateTime MDatePicker
        {
            get { return _datePicker; }
            set
            {
                if( _datePicker != value)
                {
                    _datePicker = value;
                    OnPropertyChanged(nameof(MDatePicker));
                    Debug.WriteLine("Date = " + _datePicker);
                }
            }
        }

         

        public void OnGetExchangeRatesClicked()
        {
            _xmlFileParser = new Parser();


            // Получаем выбранную дату из MDatePicker
            selectedDate = MDatePicker;
            string formattedDate = selectedDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            // Запрашиваем курсы валют на указанную дату
            if (MPickerFrom != null && MPickerTo != null)
            {                

                var currencyRateFrom =_xmlFileParser.ParseXML(formattedDate, MPickerFrom.ToString(), out float nom1);
                
                var currencyRateTo = _xmlFileParser.ParseXML(formattedDate, MPickerTo.ToString(), out float nom2);
                
                if (currencyRateFrom != 0 && currencyRateTo != 0)
                {
                    var newValue = MEntryFrom * (currencyRateFrom / nom1) / (currencyRateTo / nom2);


                    MConvertedValue = newValue.ToString();
                }
            }
        }

    }



    public partial class MainPage : ContentPage
    {


        public MainPage()
        {
            InitializeComponent();

            BindingContext = new ViewModel();
        }

        
    }
}
