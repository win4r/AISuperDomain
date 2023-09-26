const { app, BrowserWindow, Menu, MenuItem } = require('electron')
 app.whenReady().then(() => {
  const mainWindow = new BrowserWindow({
    width: 900,
    height: 700,
    webPreferences: {
      nodeIntegration: true,
      contextIsolation: false,
      webviewTag: true,
      // 启用右键菜单
      contextMenu: true
    },

    // 启动应用时窗口最大化
    maximize: true
  })


  mainWindow.once('ready-to-show', () => {
    mainWindow.maximize() // 窗口最大化
  })


  mainWindow.loadFile('index.html')
   // 定义右键菜单
  const menu = new Menu()
  menu.append(new MenuItem({
    label: '复制',
    role: 'copy'
  }))
  menu.append(new MenuItem({
    label: '粘贴',
    role: 'paste'
  }))
   // 监听右键菜单事件并显示菜单
  mainWindow.webContents.on('context-menu', (event, params) => {
    menu.popup(mainWindow, params.x, params.y)
  })
   // 移除默认菜单
  Menu.setApplicationMenu(null)
})


app.on('window-all-closed', () => {
    if (process.platform !== 'darwin') app.quit()
  })
  
  