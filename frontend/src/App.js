import { BrowserRouter as Router, Route } from "react-router-dom";

import Footer from "./components/layout/Footer";
import Header from "./components/layout/Header";

import Home from "./components/Home";
import ProductDetails from "./components/product/ProductDetails";

import "./App.css";

function App() {
  return (
    <Router>
      <div className="App">
        <Header />
        <div className="container container-fluid">
          <Route path="/" component={Home} exact />
          <Route path="/product/:id" component={ProductDetails} exact />
        </div>
        <Footer />
      </div>
    </Router>
  );
}

export default App;
