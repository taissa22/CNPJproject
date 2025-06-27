import { Injectable } from '@angular/core';
import Swal from 'sweetalert2';

@Injectable({
  providedIn: 'root'
})
export class DialogService {
  /**
   * @deprecated Use alert instead
   */
  async showAlert(title: string, message: string): Promise<void> {
    await Swal.fire({
      title: title,
      text: message,
      icon: 'success',
      showCancelButton: false,
      confirmButtonText: 'OK'
    });
  }

  /**
   * @deprecated Use err instead
   */
  async showErr(title: string, message: string): Promise<void> {
    await Swal.fire({
      title: title,
      text: message,
      icon: 'warning',
      showCancelButton: false,
      confirmButtonText: 'OK'
    });
  }

  /**
   * @deprecated Use confirm instead
   */
  async showConfirm(title: string, message: string): Promise<boolean> {
    const result = await Swal.fire<boolean>({
      title: title,
      text: message,
      icon: 'question',
      showCancelButton: true,
      cancelButtonText: 'Não',
      confirmButtonText: 'Sim'
    });
    return await result.value;
  }

  async alert(title: string, message?: string): Promise<void> {
    await Swal.fire({
      title: title,
      html: message,
      icon: 'success',
      showCancelButton: false,
      confirmButtonText: 'OK',
      allowOutsideClick: false,
      allowEscapeKey: false,
      allowEnterKey: false,
      confirmButtonColor: '#6f62b2'
    });
  }

  async info(title: string, message?: string): Promise<void> {
    await Swal.fire({
      title: title,
      html: message,
      icon: 'info',
      showCancelButton: false,
      confirmButtonText: 'OK',
      allowOutsideClick: false,
      allowEscapeKey: false,
      allowEnterKey: false,
      confirmButtonColor: '#6f62b2'
    });
  }

  async err(title: string, message?: string): Promise<void> {
    await Swal.fire({
      title: title,
      html: message,
      icon: 'warning',
      showCancelButton: false,
      confirmButtonText: 'OK',
      allowOutsideClick: false,
      allowEscapeKey: false,
      allowEnterKey: false,
      confirmButtonColor: '#6f62b2'
    });
  }

  async confirm(title: string, message?: string): Promise<boolean> {
    const result = await Swal.fire<boolean>({
      title: title,
      html: message,
      icon: 'question',
      showCancelButton: true,
      cancelButtonText: 'Não',
      confirmButtonText: 'Sim',
      allowOutsideClick: false,
      allowEscapeKey: false,
      allowEnterKey: false,
      confirmButtonColor: '#6f62b2'
    });
    return await result.value;
  }

  async confirmCustom(title: string, message?: string, showClass: any = '', permiteEnter: boolean = true, permiteEsc: boolean = true): Promise<boolean> {
    const result = await Swal.fire<boolean>({
      showClass: showClass,
      title: title,
      html: message,
      icon: 'question',
      showCancelButton: true,
      cancelButtonText: 'Não',
      confirmButtonText: 'Sim',
      allowOutsideClick: false,
      allowEscapeKey: permiteEsc,
      allowEnterKey: permiteEnter,
      confirmButtonColor: '#6f62b2'
    });
    return await result.value;
  }

  async errCustom(title: string, message?: string, showClass: any = '', permiteEnter: boolean = true, permiteEsc: boolean = true): Promise<void> {
    await Swal.fire({
      showClass: showClass,
      title: title,
      html: message,
      icon: 'warning',
      showCancelButton: false,
      confirmButtonText: 'OK',
      allowOutsideClick: false,
      allowEscapeKey: permiteEsc,
      allowEnterKey: permiteEnter,
      confirmButtonColor: '#6f62b2'
    });
  }

  async infoCustom(title: string, message?: string, showClass: any = '', permiteEnter: boolean = true, permiteEsc: boolean = true ): Promise<void> {
    await Swal.fire({
      showClass: showClass,
      title: title,
      html: message,
      icon: 'info',
      showCancelButton: false,
      confirmButtonText: 'OK',
      allowOutsideClick: false,
      allowEscapeKey: permiteEsc,
      allowEnterKey: permiteEnter,
      confirmButtonColor: '#6f62b2'
    });
  }
}
