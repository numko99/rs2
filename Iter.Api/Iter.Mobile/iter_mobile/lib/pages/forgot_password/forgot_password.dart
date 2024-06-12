import 'package:flutter/material.dart';
import 'package:iter_mobile/helpers/scaffold_messenger_helper.dart';
import 'package:iter_mobile/pages/login.dart';
import 'package:iter_mobile/pages/sign_up/sign_up_password.dart';
import 'package:iter_mobile/pages/sign_up/sign_up_token.dart';
import 'package:iter_mobile/providers/auth_provider.dart';
import 'package:provider/provider.dart';

class ForgotPassword extends StatefulWidget {
  const ForgotPassword({super.key});

  @override
  _ForgotPasswordState createState() => _ForgotPasswordState();
}

class _ForgotPasswordState extends State<ForgotPassword> {
  final _formKey = GlobalKey<FormState>();
  AuthProvider? authProvider;
  bool displayLoader = false;
  bool displayToken = false;

  final TextEditingController _emailController = TextEditingController();
  final TextEditingController _tokenController = TextEditingController();

  @override
  void initState() {
    super.initState();
    authProvider = context.read<AuthProvider>();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
          title: const Text('Reset lozinke',
              style: TextStyle(color: Colors.white)),
        ),
        body: displayLoader
            ? const Center(child: CircularProgressIndicator())
            : Padding(
                padding: const EdgeInsets.all(16.0),
                child: Form(
                  key: _formKey,
                  child: ListView(
                    children: <Widget>[
                      const Text(
                          "Molimo unesite email sa kojim ste kreirali Vaš korisnički račun.",
                          style: TextStyle(fontSize: 12)),
                      const SizedBox(height: 20),
                      TextFormField(
                        controller: _emailController,
                        enabled: !displayToken,
                        decoration: InputDecoration(
                            labelText: 'Email',
                            prefixIcon:
                                const Icon(Icons.email, color: Colors.amber),
                            border: OutlineInputBorder(
                              borderRadius: BorderRadius.circular(5.0),
                            )),
                        keyboardType: TextInputType.emailAddress,
                        validator: (value) {
                          if (value == null || value.isEmpty) {
                            return 'Polje je obavezno';
                          } else {
                            String pattern =
                                r'\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Z|a-z]{2,}\b';
                            RegExp regex = RegExp(pattern);
                            if (!regex.hasMatch(value)) {
                              return 'Unesite validnu email adresu';
                            }
                          }
                          return null;
                        },
                      ),
                      if (displayToken) ...[
                        const SizedBox(height: 20),
                        const Text(
                            "Molimo unesite token koji smo poslali na Vaš email.",
                            style: TextStyle(fontSize: 12)),
                        const SizedBox(height: 5),
                        TextFormField(
                          controller: _tokenController,
                          decoration: InputDecoration(
                              labelText: 'Token',
                              prefixIcon: const Icon(Icons.token),
                              border: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(5.0),
                              )),
                          obscureText: true,
                          validator: (value) {
                            if (value == null || value.isEmpty) {
                              return 'Molimo unesite token';
                            }

                            return null;
                          },
                        ),
                      ]
                    ],
                  ),
                ),
              ),
        bottomNavigationBar: BottomAppBar(
          child: Padding(
            padding: const EdgeInsets.all(16.0),
            child: ElevatedButton(
              onPressed: () => submit(),
              child:
                  const Text('Potvrdi', style: TextStyle(color: Colors.white)),
            ),
          ),
        ));
  }

  submit() async {
    if (_formKey.currentState?.validate() == false) {
      return;
    }

    try {
      if (!displayToken) {
        setState(() {
          displayLoader = true;
        });
        var statusCode =
            await authProvider?.sendForgotPasswordToken(_emailController.text);
        setState(() {
          displayToken = statusCode == 200;
          displayLoader = false;
        });

        if (statusCode == 404) {
          ScaffoldMessengerHelper.showCustomSnackBar(
              context: context,
              message: "Korisnički račun sa unesenim emailom nije pronađen!",
              backgroundColor: Colors.red);
        }
      }

      if (_tokenController.text == '') {
        return;
      }

      var result = await authProvider?.verifyForgotPasswordToken(
          _emailController.text, _tokenController.text);
      if (result == true) {
        var password = await Navigator.of(context).push(
            MaterialPageRoute(builder: (context) => SignUpPasswordPage()));

        if (password != null) {
          await authProvider?.resetPassword(_emailController.text, password);
          ScaffoldMessengerHelper.showCustomSnackBar(
              context: context,
              message: "Uspješno ste resetovali Vašu lozinku!",
              backgroundColor: Colors.green);

          await Navigator.of(context).pushReplacement(
              MaterialPageRoute(builder: (context) => Login()));
        } else {
          setState(() {
            displayToken = false;
            _tokenController.text = "";
          });
          return;
        }
      } else {
        ScaffoldMessengerHelper.showCustomSnackBar(
            context: context,
            message: "Uneseni token nije validan!",
            backgroundColor: Colors.red);
      }
    } catch (e) {
      ScaffoldMessengerHelper.showCustomSnackBar(
          context: context,
          message: "Oprostite, došlo je do greške",
          backgroundColor: Colors.red);
    }
  }
}
