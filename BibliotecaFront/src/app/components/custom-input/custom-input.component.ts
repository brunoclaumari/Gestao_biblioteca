import { Component, EventEmitter, Input, OnInit, Output, forwardRef } from '@angular/core';
import { AbstractControl, ControlValueAccessor, FormControl, FormGroup, NG_VALUE_ACCESSOR } from '@angular/forms';

const INPUT_FIELD_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => CustomInputComponent),
    multi: true
};

@Component({
  selector: 'app-custom-input',
  templateUrl: './custom-input.component.html',
  styleUrls: ['./custom-input.component.scss'],
  providers: [INPUT_FIELD_VALUE_ACCESSOR]
/*   providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => CustomInputComponent),
    multi: true
  }] */
})
export class CustomInputComponent implements ControlValueAccessor {


  @Input() nomeCampo:string = "";
  @Input() nomeLabel:string = "";
  @Input() maskParam:string = "";
  @Input() control: any;
  @Input() isReadOnly = false;
  @Input() type = 'text';
  @Input() lentghMaximo?:Number | undefined;
  @Input() temErros = false;

  private innerValue: any;
  field: FormControl | undefined;

  get value(){
    return this.innerValue;
  }

  set value(entry: any){
    if(entry !== this.innerValue){
      this.innerValue = entry;
      this.onChangedCb(entry)
    }
  }

  onChangedCb: ((entry: any) => void) = () =>{};
  onTouchedCb: ((entry: any) => void) = () =>{};


  constructor() { }

  writeValue(v: string) {
    this.value = v;
  }

  registerOnChange(fn: any): void {
    this.onChangedCb = fn;
  }

  registerOnTouched(fn: any) {
    this.onTouchedCb = fn
  }

  setDisabledState(isDisabled: boolean): void {
    this.isReadOnly = isDisabled;
  }

//getErrorMessage(fieldName: string) {
  get getErrorMessage() {
    this.field = this.control as FormControl;
    if (this.field?.hasError('required')) {
      return `Campo "${this.nomeCampo}" é obrigatório!`;
    }

    if (this.field?.hasError('minlength')) {
      const requiredLength:number = this.field.errors? this.field.errors['minlength']['requiredLength']:5;
      return `Tamanho mínimo tem que ser ${requiredLength} caracteres!`;
    }

    if (this.field?.hasError('maxlength')) {
      const requiredLength:number = this.field.errors? this.field.errors['maxlength']['requiredLength']:100;
      return `Tamanho máximo tem que ser ${requiredLength} caracteres!`;
    }

    if(this.nomeCampo == 'telefone' && this.field?.hasError('pattern')){
      return `Digite o telefone no formato (XX) 9XXXX-XXXX`;
    }

    return 'Campo inválido';
  }
/*   getErrorMessage(messageError:string){
    //debugger
    //this.errorMessage.emit(messageError);
  } */

}
