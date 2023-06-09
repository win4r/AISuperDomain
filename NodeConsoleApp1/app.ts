import http from 'http';
import { Parser } from 'htmlparser2';

const parser = new htmlparser.Parser({
    onopentag(name, attribs) {
        if (name === 'a' && attribs.href) {
            const href = attribs.href;
            const protocol = /^https/.test(href) ? 'https://' : 'http://';
            exec(`open ${protocol}${href.replace(/^(https|http):\/\//, '')}`); // Mac OS
            // exec(`start ${protocol}${href.replace(/^(https|http):\/\//, '')}`); // Windows
        }
    }
});

https.get(options, function(res) {
    res.pipe(parser);
});


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
const options = {
    hostname: 'www.poe.com',
    path: '/index.html',
};

const parser = new htmlparser.Parser({
    onopentag(name, attribs) {
        if (name === 'a' && attribs.href) {
            exec(`open ${attribs.href}`); // Mac OS
            // exec(`start ${attribs.href}`); // Window
        }
    }
});

http.get(options, function(res) {
    res.pipe(parser);
});


