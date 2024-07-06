import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:get_it/get_it.dart';
import 'package:provider/provider.dart';
import 'package:ui/enums/roles.dart';
import 'package:ui/services/agency_provider.dart';
import 'package:ui/services/auth_provider.dart';
import 'package:ui/services/auth_storage_provider.dart';
import '../widgets/inputField.dart';
import '../widgets/logo.dart';

class Login extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Stack(
        children: [
          BackgroundImage(),
          Center(
            child: LoginContainer(),
          ),
        ],
      ),
    );
  }
}

class LoginContainer extends StatefulWidget {
  @override
  _LoginContainerState createState() => _LoginContainerState();
}

class _LoginContainerState extends State<LoginContainer> {
  AuthProvider? _authProvider = null;

  final _formKey = GlobalKey<FormState>();
  final TextEditingController _usernameController = TextEditingController();
  final TextEditingController _passwordController = TextEditingController();

  bool displayInvalidLoginMsg = false;

  @override
  void initState() {
    super.initState();
    _authProvider = context.read<AuthProvider>();
  }

  Future<void> submit() async {
    if (_formKey.currentState?.validate() == false) {
      return;
    }

    var isValidLogin = await _authProvider?.loginUserAsync(
        _usernameController.text, _passwordController.text);
    if (isValidLogin != null && isValidLogin == true) {
      setState(() {
        displayInvalidLoginMsg = false;
      });
      var authData = AuthStorageProvider.getAuthData();

      if (authData?["role"] == Roles.admin) {
        Navigator.pushReplacementNamed(context, '/agency');
      }

      if (authData?["role"] == Roles.coordinator) {
        Navigator.pushReplacementNamed(context, '/users');
      }
    } else {
      setState(() {
        displayInvalidLoginMsg = true;
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: EdgeInsets.all(16.0),
      width: 400,
      height: 400,
      decoration: BoxDecoration(
        color: Colors.white.withOpacity(0.8),
        border: Border.all(
          color: Colors.grey[300]!,
          width: 2.0,
        ),
        borderRadius: BorderRadius.circular(10.0),
      ),
      child: Form(
        key: _formKey,
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: <Widget>[
            Logo(fontSize: 60, color: Colors.amber),
            SizedBox(height: 20),
            Visibility(
              visible: displayInvalidLoginMsg,
              // Promijenite ovo prema vašoj logici prijave
              child: Text("Korisničko ime ili lozinka nisu validni",
                  style: TextStyle(
                    color: Colors.red,
                    fontSize: 14.0,
                    fontWeight: FontWeight.bold,
                  )),
            ),
            InputField(
                label: 'Korisničko ime',
                controller: _usernameController,
                validator: (value) {
                  if (value == null || value.isEmpty) {
                    return 'Unesite korisničko ime';
                  }
                  return null;
                }),
            InputField(
              label: 'Lozinka',
              controller: _passwordController,
              isPassword: true,
              validator: (value) {
                if (value == null || value.isEmpty) {
                  return 'Unesite lozinku';
                }
                return null;
              },
            ),
            SizedBox(height: 20),
            ElevatedButton(
              style: ElevatedButton.styleFrom(
                padding: EdgeInsets.all(16.0),
              ),
              onPressed: submit,
              child: Text(
                'Prijavi se',
                style: TextStyle(color: Colors.white),
              ),
            ),
            SizedBox(height: 10),
            ForgotPasswordLink(),
          ],
        ),
      ),
    );
  }
}

class BackgroundImage extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return Container(
      decoration: BoxDecoration(
        image: DecorationImage(
          image: AssetImage('assets/backgroundImage2.jpg'),
          fit: BoxFit.cover,
        ),
      ),
    );
  }
}

class ForgotPasswordLink extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MouseRegion(
      cursor: SystemMouseCursors.click,
      child: Text(
        '',
        style: TextStyle(
          color: Colors.amber,
        ),
      ),
    );
  }
}
