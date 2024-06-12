import 'package:flutter/material.dart';
import 'package:iter_mobile/helpers/scaffold_messenger_helper.dart';
import 'package:iter_mobile/providers/auth_provider.dart';
import 'package:provider/provider.dart';

class SignUpTokenPage extends StatefulWidget {
  final Map<String, dynamic> request;

  const SignUpTokenPage({super.key, required this.request});

  @override
  _SignUpTokenPageState createState() => _SignUpTokenPageState();
}

class _SignUpTokenPageState extends State<SignUpTokenPage> {
  AuthProvider? authProvider;

  @override
  void initState() {
    super.initState();

    authProvider = context.read<AuthProvider>();
  }

  final _formKey = GlobalKey<FormState>();
  final TextEditingController _tokenController = TextEditingController();

  @override
  void dispose() {
    _tokenController.dispose();
    super.dispose();
  }

  void _submit() async {
    if (_formKey.currentState!.validate()) {
      var request = widget.request;
      request["token"] = _tokenController.text;
      var result =
          await authProvider?.verifyEmailVerificationToken(widget.request);

      if (result == false) {
        ScaffoldMessengerHelper.showCustomSnackBar(
            context: context,
            message: "Uneseni token nije validan!",
            backgroundColor: Colors.red);
      } else {
        Navigator.of(context).pop(true);
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
          title: const Text('Email verifikacija',
              style: TextStyle(color: Colors.white)),
        ),
        body: Padding(
          padding: const EdgeInsets.all(16.0),
          child: Form(
            key: _formKey,
            child: ListView(
              children: [
                const Text(
                    "Hvala vam što ste se registrirali na našu platformu.",
                    style: TextStyle(fontSize: 16)),
                const SizedBox(height: 16),
                const Text("Na Vaš email poslan je verifikacijski kod.",
                    style: TextStyle(fontSize: 16)),
                const SizedBox(height: 16),
                const Text(
                    "Molimo unesite verifikacijski kod u polje ispod kako biste dovršili proces registracije.",
                    style: TextStyle(fontSize: 16)),
                const SizedBox(height: 16),
                const Text(
                    "Ovaj korak osigurava sigurnost vašeg računa i potvrđuje vašu e-mail adresu kao valjanu.",
                    style: TextStyle(fontSize: 16)),
                const SizedBox(height: 16),
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
                const SizedBox(height: 8),
                const Text(
                    "Niste dobili verifikacijski kod? Kliknite ispod za slanje novog verifikacijskog koda.",
                    style: TextStyle(fontSize: 12)),
                const SizedBox(height: 4),
                GestureDetector(
                    onTap: () async {
                      await resendToken(context);
                    },
                    child: const Text('Ponovo pošalji token',
                        style: TextStyle(
                            color: Colors.amber, fontWeight: FontWeight.bold)))
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
        ));
  }

  Future<void> resendToken(BuildContext context) async {
    try {
      await authProvider?.resendToken(widget.request["email"]);
      ScaffoldMessengerHelper.showCustomSnackBar(
          context: context,
          message: "Novi token je poslan na Vaš email!",
          backgroundColor: Colors.green);
    } catch (e) {
      ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text('Oprostite, došlo je do greške')));
    }
  }
}
