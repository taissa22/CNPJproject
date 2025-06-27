import { Component, Input, OnInit } from '@angular/core';
import { NavigationEnd, Router, Event } from '@angular/router';
import { faThumbsDown } from '@fortawesome/free-solid-svg-icons';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { MenuNovoComponent } from '../menu-novo/menu-novo.component';

@Component({
  selector: 'app-modal-menu-novo',
  templateUrl: './modal-menu-novo.component.html',
  styleUrls: ['./modal-menu-novo.component.scss']
})
export class ModalMenuNovoComponent implements OnInit {

  constructor(private modalService: NgbModal, private router: Router) {

    this.router.events.subscribe((event: Event) => {
      if (event instanceof NavigationEnd) {
        this.url = window.location.hash == '#/home' || window.location.hash == '#/' || window.location.hash.indexOf('home') != -1 ? true : false;
      }
    });

  }

  options: any;
  url: boolean;

  ngOnInit() {
    this.url = window.location.hash == '#/home' || window.location.hash == '#/' || window.location.hash.indexOf('home') != -1 ? true : false;
  }

  open() {
    if (!this.modalService.hasOpenModals()) {
      this.modalService.open(MenuNovoComponent, { windowClass: 'fullscreen', backdropClass: 'fix-barra' });
      var boxPesquisar = document.getElementById('search-input');
      boxPesquisar.classList.remove('active');
      (window.document.querySelector('router-outlet').nextElementSibling as HTMLElement).style.display = 'none';
      // this.classSearchInput = "search-input";
      // this.opacity = "0.5";
      // this.displayLimpar = "none";
      // this.height = "0";
      // this.cursor = "normal";
    }
    else {
      this.modalService.dismissAll();
      (window.document.querySelector('router-outlet').nextElementSibling as HTMLElement).style.display = '';
    }
  }


}
