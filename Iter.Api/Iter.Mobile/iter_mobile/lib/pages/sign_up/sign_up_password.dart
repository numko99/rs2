import 'package:flutter/material.dart';

class SignUpPasswordPage extends StatefulWidget {
  @override
  _SignUpPasswordPageState createState() => _SignUpPasswordPageState();
}

class _SignUpPasswordPageState extends State<SignUpPasswordPage> {

  @override
  void initState() {
    super.initState();

  }

  final _formKey = GlobalKey<FormState>();
  final TextEditingController _newPasswordController = TextEditingController();
  final TextEditingController _confirmPasswordController =
      TextEditingController();

  @override
  void dispose() {
    _newPasswordController.dispose();
    _confirmPasswordController.dispose();
    super.dispose();
  }

  void _submit() async {
    if (_formKey.currentState!.validate()) {
      try {
        if (mounted) {
          Navigator.of(context).pop(_newPasswordController.text);
        }
      } catch (e){}
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Lozinka', style: TextStyle(color: Colors.white)),
      ),
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Form(
          key: _formKey,
          child: ListView(
            children: [
              TextFormField(
                controller: _newPasswordController,
                decoration: InputDecoration(
                    labelText: 'Lozinka',
                    prefixIcon: const Icon(Icons.key),
                    border: OutlineInputBorder(
                      borderRadius: BorderRadius.circular(5.0),
                    )),
                obscureText: true,
                validator: (value) {
                  if (value == null || value.isEmpty) {
                    return 'Molimo unesite lozinku';
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
                    labelText: 'Potvrda lozinke',
                    prefixIcon: const Icon(Icons.key),
                    border: OutlineInputBorder(
                      borderRadius: BorderRadius.circular(5.0),
                    )),
                obscureText: true,
                validator: (value) {
                  if (value == null || value.isEmpty) {
                    return 'Molimo potvrdite lozinku';
                  }
                  if (value != _newPasswordController.text) {
                    return 'Lozinke se ne podudaraju';
                  }
                  return null;
                },
              )
            ],
          ),
        ),
      ),
      bottomNavigationBar: BottomAppBar(
          child: Padding(
            padding: const EdgeInsets.all(16.0),
            child: ElevatedButton(
              onPressed: () => _submit(),
              child:
                  const Text('Potvrdi', style: TextStyle(color: Colors.white)),
            ),
          ),
        )
    );
  }
}
