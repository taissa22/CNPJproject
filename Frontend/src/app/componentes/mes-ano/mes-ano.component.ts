import { AfterViewInit, Component, DoCheck, forwardRef, HostBinding, Input, OnInit, ViewChild, } from '@angular/core';
import { ControlValueAccessor, FormBuilder, NG_VALUE_ACCESSOR } from '@angular/forms';
import { NgbDropdown, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { parseDate } from 'ngx-bootstrap';
//import { type } from 'os';


interface Idata {
  month: number;
  year: number;
}

@Component({
  selector: 'mes-ano',
  templateUrl: './mes-ano.component.html',
  styleUrls: ['./mes-ano.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => MesAnoComponent),
      multi: true
    }
  ]
})

export class MesAnoComponent implements ControlValueAccessor, AfterViewInit {

  data: Idata;
  dataTxt: string;
  separator: string;
  monthFirst: boolean;
  place: number;

  isyear:boolean=false;
  incr:number=0;

  validDate: boolean;
  fixedMonth: any;

  errorMsg = "data inválida"

  

  months: string[] = ["jan", "fev", "mar", "abr", "mai", "jun", "jul", "ago", "set", "out", "nov", "dez"];
  // Allow the input to be disabled, and when it is make it somewhat transparent.
  @Input() disabled = false;
  @Input() mask = "mm/yyyy";
  @Input() id = ""


  @ViewChild('calendarPanel', {static: false}) calendar: NgbDropdown; 

  constructor() {
    this.separator = this.mask.replace(/m|y|M/gi, "");
    this.monthFirst = this.mask.indexOf('y') > 0;
    this.place = this.mask.indexOf(this.separator);

  }

  ngAfterViewInit(){
    
      this.fixDate();
    
  }


  change(value: string) {
    value=this.separator==" "?value.replace(/\.|-|\//," "):
          this.separator=="/"?value.replace(/\.|-| /,"/"):
          this.separator=="-"?value.replace(/\.| |\//,"-"):
          value.replace(/.| |\/ /,"-");

    let lastchar = value.substr(value.length - 1);
    if (lastchar == this.separator && value.length <= this.place) {
      if (this.monthFirst) {
        value = "0" + value;
      }
    }
    if (value.length > this.place && value.indexOf(this.separator) < 0) {
      value = value.substr(0, value.length - 1) + this.separator + lastchar;
    }
    this.dataTxt = value;
    let items = value.split(this.separator);
    
    if (items.length == 2) {
      let year = this.monthFirst ? items[1] : items[0];
      let month = this.monthFirst ? items[0] : items[1];
      let imonth = this.months.indexOf(month);
      if ((imonth) < 0)
        imonth = parseInt(month);
      else
        imonth = imonth + 1;

      let iyear = parseInt(year);
      if (iyear < 100)
        iyear = iyear + 2000;
      this.data = {
        year: iyear,
        month: imonth
      }
      this.incr=this.getIncr(this.data.year);
    }
    
    this.writeValue(this.data);
    
    

  }

  validateDate(){
  
    let fullDate = parseDate(this.data.year + this.fixedMonth +'01').toString();

    
    if (fullDate == 'NaN' || fullDate == 'Invalid Date' || this.data.year.toString().length === 3 ) {
      this.validDate = false;
    }else{
      this.validDate = true;
    }
   
  }

  fixDate(){
    let typedYear = this.data.year.toString();
    let typedMonth = this.data.month.toString();
    
    
    
    if(this.validDate){
      

      if(typedMonth.length === 1){
        typedMonth = '0'+ typedMonth;
      }

      this.dataTxt = this.fixedMonth + this.separator + typedYear;

    }

    if(typedYear === 'NaN'){
      let thisMoment = new Date;
      typedYear = thisMoment.getFullYear().toString();
      this.data.year = thisMoment.getFullYear();



      this.dataTxt = this.fixedMonth + this.separator + typedYear;
    }

    this.writeValue(this.data);

    
  }

  selectYearMonth($event,index:number)
  {
    if (this.isyear)
    {
      $event.stopPropagation();
      this.data.year=index+this.incr;
      this.dataTxt=this.formatData(this.data);
      this.isyear=false;
      this.incr=this.getIncr(this.data.year);
    }
    else{
    this.data.month=index+1;
    this.dataTxt=this.formatData(this.data);
    } 
    this.writeValue(this.data);
    
    
  }
  showYear($event:any,show:boolean)
  {
    $event.stopPropagation();
    this.isyear=!this.isyear;
  }
  addYear($event:any,incr:number)
  {
    $event.stopPropagation(); 
    let year=this.isyear?this.data.year+10*incr:this.data.year+incr;
    this.data.year=year;
    this.incr=this.getIncr(year);
    this.dataTxt=this.formatData(this.data);

    
    this.writeValue(this.data);
    
    
  }
  onChange = (data: Idata) => {
    this.data = data;
    this.dataTxt = this.monthFirst ? "" + data.month + this.separator + data.year :
      "" + data.year + this.separator + data.month;
      this.incr=this.getIncr(this.data.year);
  };

  getIncr(year:number):number
  {
    return (year-year%10)-1;
  }
  formatData(data:Idata):string
  {
    let monthTxt=data.month<10? "0"+data.month:"" + data.month;
    return  this.monthFirst ?  monthTxt+ this.separator + data.year :
    "" + data.year + this.separator + monthTxt

  }
  // Function to call when the input is touched (when a star is clicked).
  onTouched = () => { };

  writeValue(data: Idata): void {
    this.data = data;
    this.onChange(this.data)

    let originalMonthLenght = this.data.month.toString().length;
    

    if(originalMonthLenght == 1){
      this.fixedMonth =  '0' + this.data.month;
    }
    else if(originalMonthLenght == 2){
      this.fixedMonth =  ''+this.data.month;
    }
    else{}

    this.validateDate();
  }

  // Allows Angular to register a function to call when the model (rating) changes.
  // Save the function as a property to call later here.
  registerOnChange(fn: (data: Idata) => void): void {
    this.onChange = fn;
  }

  // Allows Angular to register a function to call when the input has been touched.
  // Save the function as a property to call later here.
  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  // Allows Angular to disable the input.
  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  

  
}