import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeleteMovie } from './delete-movie';

describe('DeleteMovie', () => {
  let component: DeleteMovie;
  let fixture: ComponentFixture<DeleteMovie>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DeleteMovie]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DeleteMovie);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
