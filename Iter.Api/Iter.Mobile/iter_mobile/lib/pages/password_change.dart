import 'package:flutter/material.dart';
import 'package:iter_mobile/providers/user_provider.dart';
import 'package:provider/provider.dart';

class ChangePasswordPage extends StatefulWidget {
  @override
  _ChangePasswordPageState createState() => _ChangePasswordPageState();
}

class _ChangePasswordPageState extends State<ChangePasswordPage> {
  
   UserProvider? _userProvider;

  @override
  void initState() {
    super.initState();

    _userProvider = context.read<UserProvider>();
  }

  final _formKey = GlobalKey<FormState>();
  final TextEditingController _oldPasswordController = TextEditingController();
  final TextEditingController _newPasswordController = TextEditingController();
  final TextEditingController _confirmPasswordController =
      TextEditingController();

  @override
  void dispose() {
    _oldPasswordController.dispose();
    _newPasswordController.dispose();
    _confirmPasswordController.dispose();
    super.dispose();
  }

void _submit() async {
    if (_formKey.currentState!.validate()) {
      try {
        await _userProvider?.updatePassword(_oldPasswordController.text, _newPasswordController.text);
        if (mounted) {
          Navigator.of(context).pop(true);
        } 
      } catch (e) {
        _showErrorDialog('Trenutna lozinka nije ispravna.');

      }
    }
  }

  void _showErrorDialog(String message) {
    showDialog(
      context: context,
      builder: (ctx) => AlertDialog(
        title: Text('Greška'),
        content: Text(message),
        actions: <Widget>[
          TextButton(
            child: Text('U redu'),
            onPressed: () {
              Navigator.of(ctx).pop();
            },
          ),
        ],
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Promjena lozinke', style: TextStyle(color: Colors.white)),
      ),
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Form(
          key: _formKey,
          child: ListView(
            children: [
              TextFormField(
                controller: _oldPasswordController,
                decoration: InputDecoration(
                    labelText: 'Trenutna lozinka',
                    prefixIcon: const Icon(Icons.key),
                    border: OutlineInputBorder(
                      borderRadius: BorderRadius.circular(5.0),
                    )),
                obscureText: true,
                validator: (value) {
                  if (value == null || value.isEmpty) {
                    return 'Molimo unesite trenutnu lozinku';
                  }
                  return null;
                },
              ),
              SizedBox(height: 20),
              TextFormField(
                controller: _newPasswordController,
                decoration: InputDecoration(
                    labelText: 'Nova lozinka',
                    prefixIcon: const Icon(Icons.key),
                    border: OutlineInputBorder(
                      borderRadius: BorderRadius.circular(5.0),
                    )),
                obscureText: true,
                validator: (value) {
                  if (value == null || value.isEmpty) {
                    return 'Molimo unesite novu lozinku';
                  }
                    if (value.length < 8) {
                    return 'Lozinka mora imati najmanje 8 karaktera';
                  }
                  return null;
                },
              ),
              SizedBox(height: 20),
              TextFormField(
                controller: _confirmPasswordController,
                decoration: InputDecoration(
                    labelText: 'Potvrda nove lozinke',
                    prefixIcon: const Icon(Icons.key),
                    border: OutlineInputBorder(
                      borderRadius: BorderRadius.circular(5.0),
                    )),
                obscureText: true,
                validator: (value) {
                  if (value == null || value.isEmpty) {
                    return 'Molimo potvrdite novu lozinku';
                  }
                  if (value != _newPasswordController.text) {
                    return 'Lozinke se ne podudaraju';
                  }
                  return null;
                },
              ),
              SizedBox(height: 20),
              ElevatedButton(
                onPressed: _submit,
                child: Text('Spremi promjene', style: TextStyle(color: Colors.white)),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
