import 'package:flutter/material.dart';
import 'package:flutter_form_builder/flutter_form_builder.dart';
import 'package:form_builder_validators/form_builder_validators.dart';
import 'package:provider/provider.dart';
import 'package:ui/helpers/scaffold_messenger_helper.dart';
import 'package:ui/services/user_provider.dart';

class ChangePasswordModal extends StatefulWidget {
  @override
  _ChangePasswordModalState createState() => _ChangePasswordModalState();
}

class _ChangePasswordModalState extends State<ChangePasswordModal> {
  UserProvider? _userProvider;

  @override
  void initState() {
    super.initState();
    _userProvider = context.read<UserProvider>();
  }

  final _formKey = GlobalKey<FormBuilderState>();

  Future<void> _submit() async {
    if (_formKey.currentState?.saveAndValidate() ?? false) {
      final oldPassword = _formKey.currentState?.value['oldPassword'];
      final newPassword = _formKey.currentState?.value['newPassword'];

      try {
        var res = await _userProvider?.updatePassword(
            oldPassword, newPassword);

        if (res) {
          ScaffoldMessengerHelper.showCustomSnackBar(
              context: context,
              message: "Uspješno ste promijenili lozinku",
              backgroundColor: Colors.green);
        } else {
          ScaffoldMessengerHelper.showCustomSnackBar(
              context: context,
              message: "Trenutna lozinka nije ispravna!",
              backgroundColor: Colors.red);
        }
      } catch (e) {
        ScaffoldMessengerHelper.showCustomSnackBar(
            context: context,
            message: "Došlo je do greške",
            backgroundColor: Colors.red);
      } finally {
        if (mounted) {
          Navigator.of(context).pop(true);
        }
      }
    }
  }

  void _showErrorDialog(String message) {
    showDialog(
      context: context,
      builder: (ctx) => AlertDialog(
        title: const Text('Greška'),
        content: Text(message),
        actions: <Widget>[
          TextButton(
            child: const Text('U redu'),
            onPressed: () {
              if (mounted) {
                Navigator.of(ctx).pop();
              }
            },
          ),
        ],
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return AlertDialog(
      title: const Column(
        children: [
          Text('Promijena lozinke'),
          SizedBox(height: 10),
          Icon(
            Icons.key,
            color: Colors.amber,
            size: 50,
          ),
        ],
      ),
      contentPadding: const EdgeInsets.fromLTRB(30.0, 5, 30.0, 40),
      content: SingleChildScrollView(
        child: Container(
          width: 600.0,
          child: FormBuilder(
            key: _formKey,
            child: Column(
              children: [
                FormBuilderTextField(
                  name: 'oldPassword',
                  decoration: InputDecoration(
                    labelText: 'Trenutna lozinka',
                    prefixIcon: const Icon(Icons.key),
                  ),
                  obscureText: true,
                  validator: FormBuilderValidators.compose([
                    FormBuilderValidators.required(),
                  ]),
                ),
                const SizedBox(height: 20),
                FormBuilderTextField(
                  name: 'newPassword',
                  decoration: InputDecoration(
                    labelText: 'Nova lozinka',
                    prefixIcon: const Icon(Icons.key),
                  ),
                  obscureText: true,
                  validator: FormBuilderValidators.compose([
                    FormBuilderValidators.required(),
                    FormBuilderValidators.minLength(8),
                  ]),
                ),
                const SizedBox(height: 20),
                FormBuilderTextField(
                  name: 'confirmPassword',
                  decoration: InputDecoration(
                    labelText: 'Potvrda nove lozinke',
                    prefixIcon: const Icon(Icons.key),
                  ),
                  obscureText: true,
                  validator: FormBuilderValidators.compose([
                    FormBuilderValidators.required(),
                    (val) {
                      if (val != _formKey.currentState?.value['newPassword']) {
                        return 'Lozinke se ne podudaraju';
                      }
                      return null;
                    },
                  ]),
                ),
                const SizedBox(height: 20),
                 Row(
                  mainAxisAlignment: MainAxisAlignment.end,
                  children: [
                    ElevatedButton(
                      onPressed: () {
                        Navigator.of(context).pop();
                      },
                      style: const ButtonStyle(
                        backgroundColor:
                            WidgetStatePropertyAll<Color>(Colors.white70),
                      ),
                      child: const Text('Odustani'),
                    ),
                    const SizedBox(width: 8),
                    ElevatedButton(
                      onPressed: () async {
                        if (_formKey.currentState?.saveAndValidate() ?? false) {
                          await _submit();
                        }
                      },
                      child: const Text(
                        'Sačuvaj',
                        style: TextStyle(color: Colors.white),
                      ),
                    ),
                  ],
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }
}
