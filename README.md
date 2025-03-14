# React Project Commands Documentation
 
## Setting Up the Project

### 1. Install Vite
Vite helps us quickly create a React app. Run the following command:
```bash
npm create vite@latest
```
- **Project name**: `frontend-app-react-ts`
- Select `react` and `typescript`.

### 2. Open the Project in Visual Studio Code
Open the project folder in Visual Studio Code using the command:
```bash
code .
```

### 3. Initialize Node Modules
Run the following command to install the necessary node modules:
```bash
npm i
```

### 4. Install Required Packages
Install the essential packages for your project:
```bash
npm i @hookform/resolvers axios chart.js moment react-chartjs-2 react-hook-form react-hot-toast react-icons react-router-dom yup
```

---

## Setting Up Tailwind CSS

### 5. Install and Configure Tailwind CSS
1. Visit the [Tailwind CSS Installation Guide](https://tailwindcss.com/docs/installation).
2. Under **Framework Guides**, select **Vite**.
3. Copy and run the following commands:
   ```bash
   npm install -D tailwindcss postcss autoprefixer
   npx tailwindcss init -p
   ```
4. Open the `tailwind.config.js` file and replace its contents with:
   ```js
   export default {
     content: ["./index.html", "./src/**/*.{js,ts,jsx,tsx}"],
     theme: {
       extend: {},
     },
     plugins: [],
   }
   ```
5. Open the `index.css` file located at `src/index.css`:
   - Remove all existing CSS.
   - Delete the `app.css` file and any related imports from `app.tsx`.
   - Add the following lines to `index.css`:
     ```css
     @tailwind base;
     @tailwind components;
     @tailwind utilities;
     ```

---

## Updating App.tsx

### 6. Replace App.tsx Code
1. Open the `App.tsx` file.
2. Remove all the existing code and replace it with:
   ```tsx
   import React from 'react';

   const App = () => {
     return (
       <div className='bg-red-500 p-8'>
         App
       </div>
     );
   };

   export default App;
   ```

---
## Application Hosting Configuration 
  open file package.json 
```bash
  "scripts": {
    "dev": "vite --port=3000",
    "build": "tsc -b && vite build",
    "lint": "eslint .",
    "preview": "vite preview"
  }
```  
## Running the Application

### 7. Start the Development Server
Run the following command to start the development server:
```bash
npm run dev
```

---

## Recommended Extensions for Visual Studio Code

Install these extensions for an improved development experience:
1. **Tailwind CSS IntelliSense**
2. **ES7+ React/Redux/React-Native Snippets**
3. **Prettier - Code Formatter**
