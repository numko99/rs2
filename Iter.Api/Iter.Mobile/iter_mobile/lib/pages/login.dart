import 'package:flutter/material.dart';
import 'package:iter_mobile/models/auth_request.dart';
import 'package:iter_mobile/providers/auth_provider.dart';
import 'package:iter_mobile/widgets/layout.dart';
import 'package:iter_mobile/widgets/logo.dart';
import 'package:provider/provider.dart';
// import 'package:provider/provider.dart';

class Login extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: SingleChildScrollView(
        child: Center(
          child: LoginContainer(),
        ),
      ),
    );
  }
}

class LoginContainer extends StatefulWidget {
  @override
  _LoginContainerState createState() => _LoginContainerState();
}

class _LoginContainerState extends State<LoginContainer> {
  final _formKey = GlobalKey<FormState>();
  final TextEditingController _usernameController = TextEditingController();
  final TextEditingController _passwordController = TextEditingController();

  bool displayInvalidLoginMsg = false;
    AuthProvider? _authProvider = null;

    @override
  void initState() {
    super.initState();
    _authProvider = context.read<AuthProvider>();
  }

  Future<void> submit() async {
    if (_formKey.currentState?.validate() == false) {
      return;
    }

    var isValidLogin = await _authProvider!.loginUserAsync(AuthRequest(_usernameController.text, _passwordController.text));
    if (isValidLogin != null && isValidLogin == true) {
      setState(() {
        displayInvalidLoginMsg = false;
      });
      Navigator.of(context)
          .pushReplacement(MaterialPageRoute(builder: (context) => const Layout()));
    } else {
      setState(() {
        displayInvalidLoginMsg = true;
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return Center(
      child: Container(
       height: MediaQuery.of(context).size.height,
        padding: const EdgeInsets.all(32.0),
        decoration: BoxDecoration(
          color: Colors.white.withOpacity(0.8),
          border: Border.all(
            color: Colors.grey.shade300,
            width: 2.0,
          ),
          borderRadius: BorderRadius.circular(10.0),
        ),
        child: Form(
          key: _formKey,
          child: Column(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: <Widget>[
              Expanded(flex: 1, child: Center(child: Logo(fontSize: 60, color: Colors.amber))),
              Expanded(
                flex: 2,
                child: Column(
                  children: [
                    Visibility(
                      visible: displayInvalidLoginMsg,
                      child: const Text("Korisničko ime ili lozinka nisu validni",
                          style: TextStyle(
                            color: Colors.red,
                            fontSize: 14.0,
                            fontWeight: FontWeight.bold,
                          )),
                    ),
                    const SizedBox(height: 30),
                    TextFormField(
                        decoration: InputDecoration(
                            labelText: "Korisničko ime",
                            prefixIcon: const Icon(Icons.person),
                            border: OutlineInputBorder(
                              borderRadius: BorderRadius.circular(
                                  5.0), // Možete prilagoditi radijus zaobljenja
                            )),
                        controller: _usernameController,
                        validator: (value) {
                          if (value == null || value.isEmpty) {
                            return 'Unesite korisničko ime';
                          }
                          return null;
                        }),
                    const SizedBox(height: 20),
                    TextFormField(
                      decoration: InputDecoration(
                          labelText: "Lozinka",
                          prefixIcon: const Icon(Icons.lock),
                          border: OutlineInputBorder(
                            borderRadius: BorderRadius.circular(
                                5.0), // Možete prilagoditi radijus zaobljenja
                          )),
                      controller: _passwordController,
                      obscureText: true,
                      validator: (value) {
                        if (value == null || value.isEmpty) {
                          return 'Unesite lozinku';
                        }
                        return null;
                      },
                    ),
                    const SizedBox(height: 20),
                    SizedBox(
                      width: double.infinity,
                      child: ElevatedButton(
                        style: ElevatedButton.styleFrom(
                          padding:
                              const EdgeInsets.symmetric(vertical: 12.0, horizontal: 24.0),
                        ),
                        onPressed: submit,
                        child: const Text(
                          'Prijavi se',
                          style: TextStyle(color: Colors.white),
                        ),
                      ),
                    ),
                    const SizedBox(height: 10),
                    GestureDetector(
                      onTap: () {
                        // Implementirajte funkcionalnost za "zaboravljenu lozinku" ovdje
                      },
                      child: const Text(
                        'Zaboravljena lozinka?',
                        style: TextStyle(
                          color: Colors.amber,
                        ),
                      ),
                    ),
                  ],
                ),
              ),
              Row(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  const Text("Nemate korisnički račun?"),
                  const SizedBox(width: 5),
                  GestureDetector(
                    onTap: () {
                      // Implementirajte funkcionalnost za "zaboravljena lozinka" ovdje
                    },
                    child: const Text(
                      'Registrujte se?',
                      style: TextStyle(
                        color: Colors.amber,
                      ),
                    ),
                  ),
                ],
              )
            ],
          ),
        ),
      ),
    );
  }
}
