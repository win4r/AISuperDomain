import http from 'http';
import { Parser } from 'htmlparser2';

http.get('http://poe.com', (res) => {
  let html = '';

  res.on('data', (chunk) => {
    html += chunk;
  });

  res.on('end', () => {
    const handler = new Parser({
      onopentag(name, attrs) {
        console.log(`Found opening tag: ${name}`);
      },
      ontext(text) {
        console.log(`Found text: ${text}`);
      },
      onclosetag(name) {
        console.log(`Found closing tag: ${name}`);
      }
    }, { decodeEntities: true });

    handler.write(html);
    handler.end();
  });
}).on('error', (e) => {
  console.error(e);
});

http.get(url, (res) => {
  let rawData = '';
  res.on('data', (chunk) => {
    rawData += chunk;
  });
  res.on('end', () => {
    const parser = new htmlparser.Parser({
      ontext: function(text) {
        console.log('Text: ' + text);
      },
      onopentag: function(name, attribs) {
        console.log('Open tag: ' + name);
      },
      onclosetag: function(name) {
        console.log('Close tag: ' + name);
      },
      onerror: function(error) {
        console.error('Error: ' + error);
      }
    }, {decodeEntities: true});
    parser.write(rawData);
    parser.end();
  });
}).on('error', (error) => {
  console.error('Error: ' + error);
});

