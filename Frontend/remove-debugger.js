const fs = require('fs');
const path = require('path');

const directoryPath = path.join(__dirname, 'src');

function removeDebuggerStatements(dir) {
  fs.readdirSync(dir).forEach(file => {
    const filePath = path.join(dir, file);
    if (fs.statSync(filePath).isDirectory()) {
      removeDebuggerStatements(filePath);
    } else if (filePath.endsWith('.ts')) {
      let content = fs.readFileSync(filePath, 'utf8');
      content = content.replace(/debugger;?/g, '');
      fs.writeFileSync(filePath, content, 'utf8');
    }
  });
}

removeDebuggerStatements(directoryPath);
