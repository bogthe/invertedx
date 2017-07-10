import { InvertedxWebPage } from './app.po';

describe('invertedx-web App', () => {
  let page: InvertedxWebPage;

  beforeEach(() => {
    page = new InvertedxWebPage();
  });

  it('should display welcome message', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('Welcome to app!!');
  });
});
