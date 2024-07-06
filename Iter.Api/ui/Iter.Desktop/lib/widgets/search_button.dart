import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';

class SearchButton extends StatelessWidget {
  final VoidCallback onSearch;

  const SearchButton({super.key, required this.onSearch});

  @override
  Widget build(BuildContext context) {
    return ElevatedButton(
      onPressed: onSearch,
      style: ButtonStyle(
        backgroundColor: MaterialStateProperty.resolveWith<Color>(
          (Set<MaterialState> states) {
            if (states.contains(MaterialState.pressed)) {
              return Colors.amber.withOpacity(0.5);
            } else {
              return Colors.white;
            }
          },
        ),
        side: MaterialStateProperty.all(const BorderSide(color: Colors.amber)),
      ),
      child: const Text(
        "Pretraga",
        style: TextStyle(color: Colors.amber),
      ),
    );
  }
}
