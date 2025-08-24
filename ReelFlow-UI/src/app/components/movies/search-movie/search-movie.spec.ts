import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchMovie } from './search-movie';

describe('SearchMovie', () => {
  let component: SearchMovie;
  let fixture: ComponentFixture<SearchMovie>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SearchMovie]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SearchMovie);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
