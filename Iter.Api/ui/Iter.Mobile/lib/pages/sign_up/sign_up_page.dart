import 'package:flutter/material.dart';
import 'package:flutter_multi_formatter/formatters/masked_input_formatter.dart';
import 'package:intl/intl.dart';
import 'package:iter_mobile/helpers/scaffold_messenger_helper.dart';
import 'package:iter_mobile/models/user.dart';
import 'package:iter_mobile/pages/sign_up/login_page.dart';
import 'package:iter_mobile/pages/sign_up/sign_up_password_page.dart';
import 'package:iter_mobile/pages/sign_up/sign_up_token_page.dart';
import 'package:iter_mobile/providers/auth_provider.dart';
import 'package:iter_mobile/providers/user_provider.dart';
import 'package:provider/provider.dart';

class SignUpPage extends StatefulWidget {
  const SignUpPage({super.key});

  @override
  _SignUpPageState createState() => _SignUpPageState();
}

class _SignUpPageState extends State<SignUpPage> {
  final _formKey = GlobalKey<FormState>();
  AuthProvider? authProvider;
  bool displayLoader = false;

  final TextEditingController _nameController = TextEditingController();
  final TextEditingController _surnameController = TextEditingController();
  final TextEditingController _residencePlaceController = TextEditingController();
  final TextEditingController _emailController = TextEditingController();
  final TextEditingController _phoneController = TextEditingController();
  final TextEditingController _birthDateController = TextEditingController();

  @override
  void initState() {
    super.initState();
    authProvider = context.read<AuthProvider>();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
          title: const Text('Registracija profila', style: TextStyle(color: Colors.white)),
        ),
        body: displayLoader
            ? const Center(child: CircularProgressIndicator())
            : Padding(
                padding: const EdgeInsets.all(16.0),
                child: Form(
                  key: _formKey,
                  child: ListView(
                    children: <Widget>[
                      TextFormField(
                        controller: _nameController,
                        decoration: InputDecoration(
                          labelText: 'Ime',
                          prefixIcon: const Icon(Icons.person, color: Colors.amber),
                          border: OutlineInputBorder(
                              borderRadius: BorderRadius.circular(5.0),
                            )
                          ),
                        validator: (value) {
                          if (value == null || value.isEmpty) {
                            return 'Polje je obavezno';
                          }
                          return null;
                        },
                      ),
                      const SizedBox(height: 20),
                      TextFormField(
                        controller: _surnameController,
                        decoration: InputDecoration(
                            labelText: 'Prezime',
                            prefixIcon: const Icon(Icons.person, color: Colors.amber),
                            border: OutlineInputBorder(
                              borderRadius: BorderRadius.circular(5.0),
                            )),
                        validator: (value) {
                          if (value == null || value.isEmpty) {
                            return 'Polje je obavezno';
                          }
                          return null;
                        },
                      ),
                      const SizedBox(height: 20),
                      TextFormField(
                        controller: _birthDateController,
                        decoration: InputDecoration(
                          labelText: 'Datum rođenja',
                          prefixIcon: const Icon(Icons.calendar_month,
                              color: Colors.amber),
                          border: OutlineInputBorder(
                            borderRadius: BorderRadius.circular(5.0),
                          ),
                        ),
                        readOnly: true,
                        onTap: () async {
                          DateTime? pickedDate = await showDatePicker(
                            context: context,
                            initialDate: DateTime.now(),
                            firstDate: DateTime(1900),
                            lastDate: DateTime.now(),
                            builder: (BuildContext context, Widget? child) {
                              return Theme(
                                data: ThemeData.light(),
                                child: child!,
                              );
                            },
                          );

                          if (pickedDate != null) {
                            String formattedDate =
                                DateFormat('dd.MM.yyyy').format(pickedDate);
                            _birthDateController.text = formattedDate;
                          }
                        },
                        validator: (value) {
                          if (value == null || value.isEmpty) {
                            return 'Polje je obavezno';
                          }
                          return null;
                        },
                        keyboardType: TextInputType.none,
                      ),
                      const SizedBox(height: 20),
                      TextFormField(
                        controller: _residencePlaceController,
                        decoration:
                            InputDecoration(
                            labelText: 'Mjesto prebivališta',
                            prefixIcon: const Icon(Icons.location_city,
                                color: Colors.amber),
                            border: OutlineInputBorder(
                              borderRadius: BorderRadius.circular(5.0),
                            )),
                        validator: (value) {
                          if (value == null || value.isEmpty) {
                            return 'Polje je obavezno';
                          }
                          return null;
                        },
                      ),
                      const SizedBox(height: 20),
                      TextFormField(
                        controller: _emailController,
                        decoration: InputDecoration(
                            labelText: 'Email',
                            prefixIcon: const Icon(Icons.email, color: Colors.amber),
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
                      const SizedBox(height: 20),
                      TextFormField(
                        controller: _phoneController,
                        inputFormatters: [
                                MaskedInputFormatter('000-000-0000'),
                              ],
                        decoration: InputDecoration(
                            labelText: 'Broj telefona',
                            prefixIcon: const Icon(Icons.phone, color: Colors.amber,),
                            border: OutlineInputBorder(
                              borderRadius: BorderRadius.circular(5.0),
                            )),
                        keyboardType: TextInputType.phone,
                        validator: (value) {
                          if (value == null || value.isEmpty) {
                            return 'Polje je obavezno';
                          }
                          final regex = RegExp(r'^\d{3}-\d{3}-\d{3,4}$');
                          if (!regex.hasMatch(value)) {
                            return 'Neispravan format';
                          }
                          return null;
                        },
                      ),
                    ],
                  ),
                ),
              ),
        bottomNavigationBar: BottomAppBar(
          child: Padding(
            padding: const EdgeInsets.all(16.0),
            child: ElevatedButton(
              onPressed: () => submit(),
              child: const Text('Potvrdi',
                  style: TextStyle(color: Colors.white)),
            ),
          ),
        ));
  }

  submit() async {
    if (_formKey.currentState?.validate() == false) {
      return;
    }

    var password = await Navigator.of(context)
        .push(MaterialPageRoute(builder: (context) => SignUpPasswordPage()));

    if (password == null){
      return;
    }
    
    setState(() {
      displayLoader = true;
    });

    Map<String, dynamic> request = {
      'firstName': _nameController.text,
      'lastName': _surnameController.text,
      'residencePlace': _residencePlaceController.text,
      'email': _emailController.text,
      'phoneNumber': _phoneController.text,
      'birthDate': _birthDateController.text,
      'password': password
    };

    Map<bool, String>? result= await authProvider?.registerUserAsync(request);
    setState(() {
      displayLoader = false;
    });

    if (result!.containsKey(true)) {
       var result = await Navigator.of(context)
          .push(MaterialPageRoute(builder: (context) => SignUpTokenPage(request: request)));

      if (result == true){
          ScaffoldMessengerHelper.showCustomSnackBar(
            context: context,
            message: "Uspješno ste kreirali korisnički račun!",
            backgroundColor: Colors.green);

        await Navigator.of(context).pushReplacement(MaterialPageRoute(
            builder: (context) => Login()));
      }
    }

    if (result.containsValue("DuplicateEmail")) {
       ScaffoldMessengerHelper.showCustomSnackBar(
          context: context,
          message: "Korisnik račun sa unesenim emailom već postoji",
          backgroundColor: Colors.red);
    }


  }
}
